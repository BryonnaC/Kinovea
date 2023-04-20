using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Emgu.CV.CvEnum;
using Emgu.CV;
using static System.Math;
// nuget => emgu.cv.bitmap
// if you're still getting errors, go on a rampage uninstalling and reinstalling in different ways til it works (somehow not really a joke)

namespace AnalysisSystemFinal
{
    // structs for returning information
    // these are intended to be expanded if we want to return more stuff

    #region TARGET
    public struct Target // using TM_SQDIFF
    {
        public Point location; // minLoc, adjusted per screen as minLoc has a default origin of (0,0)
        public double value; // minVal for client debugging
        public bool detected; // minVal < threshold
        public double confidence; // (1 - minVal/threshold) * 100 for perc representation of detection confidence cuz why not
        public double threshold; // THRESHOLD for this Target
        public int screenId; // screen id this Target was found on

        public Target(Point minLoc, double minVal, double threshold, int screenId)
        {
            // if we know the screen id there's no reason to collect the origins of EVERY screen 4head
            Point[] screenOrigins = (from screen in (IEnumerable<Screen>)Screen.AllScreens select new Point(screen.Bounds.X, screen.Bounds.Y)).ToArray();
            location = new Point(screenOrigins[screenId].X + minLoc.X, screenOrigins[screenId].Y + minLoc.Y);
            value = minVal;
            this.threshold = threshold;
            this.screenId = screenId;
            confidence = Round(minVal > threshold ? 0 : ((1 - minVal / threshold) * 100), 3);
            detected = minVal < threshold;
        }

        // ui props sorta deconstruct
        public void Deconstruct(out Point location, out double value,
                                out bool detected, out double confidence)
        {
            location = this.location;
            value = this.value;
            detected = this.detected;
            confidence = this.confidence;
        }
        // debug deconstruct
        public void Deconstruct(out double threshold, out int screenId)
        {
            threshold = this.threshold;
            screenId = this.screenId;
        }
    }
    #endregion

    #region EVENT_INFO
    public struct EventInfo
    {
        public Target?[] targets; // SparkVue target information
        public double delay; // approx. delay between programmatically starting Kinovea and clicking record in SparkVue
        public string fileName;

        public EventInfo(Target?[] targets, double delay, string fileName = "")
        {
            this.targets = targets;
            this.delay = delay;
            this.fileName = fileName;
        }

        // non-nullable constructor
        public EventInfo(Target[] targets, double delay, string fileName = "")
        {
            this.targets = (from target in targets select (Target?)target).ToArray();
            this.delay = delay;
            this.fileName = fileName;
        }

        public void Deconstruct(out Target?[] targets, out double delay)
        {
            targets = this.targets;
            delay = this.delay;
        }
    }
    #endregion

    public static class FakeUser
    {
        #region STATIC VARS
        private static readonly Screen[] screens = Screen.AllScreens;

        // old SPARKvue templates
        //new TemplateData("iVBORw0KGgoAAAANSUhEUgAAAEwAAAAlCAYAAADobA+5AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAP5SURBVGhD7ZlNT1NBFIbv36BUJXUDCTFYY0wAsQkbmjRu2ahsEERbqlAEAlRQoAViMNSaiHyoC03AKImLxujKRKKJ+pfGeYc717nTU3qh7YVJunh6pzNzznvO2+kHF6th5A2r4x2r4QEfeKRxZJM1T2ZZeHaMdSzcZt3Lvazn6XUWW42KK55jHuvYh/1UHpPxZNi5sTxrS0+wa9lb3Jwez2A/4hBP5TWRsoa1zqRZJHuTNMQriEceKr9pWAH+QNGUesEuzyV5w9EiA45HVORDXkrPFKzAfT7QCI2tsfb5QaLpykFe5Kd0TYAb9poP/tOUytfMLAnyQ0fXNgErkOQDhYO3Id1oNYGOrm0CLsNap9K8mWp9ZpUjKvRUfRNwDGsazVf8bXhUoAddtaDTjhUY5gNO28wE2VStga6swQS4YdssmHzFIhl/T5cEutBHHSYgDGsez5DN+AX09cLAyqd99vbrX4etL39YX75A7vULYVg4nSIb8Qvo64XBrPXCbxZb3itak6Tff6+6iYnNb+LFwZVatwKJbdYx30824hfQRx2S2NKeMAuGqPM6LsOI9eOQ2LAN41dqXRjWvdRLNuIX0FeLggEwYuXjvmteBWvk25WvScPlWu7zL3Z1bteJlevIIQ3CnsyHH06MRDfOCsS37Vs0dDN+AH3UoSINcZrV1kH6nXLC7DmYgRhc8Vyaj73qHhgGYJKcB64TpsxLhGH+/VgtRZQsTj1FasMSyjAdmA0DxWm156Rh1ItR1rBG/hA94RMGfdRBIZuTp61rdtdZcwx7XnDFqGA/4tRYmRMm6vtVw/Q1IAw7DZ9hemE6MAeNqE2WMgzPMY/9kuoZdm+LdT452W9J6KOOw+ia3VFOyo6Ycxlm75MNCzNKxMWyimF2nMRlmLYGhGHhmVGyEb+Avl6YjlfDYALMgCml4io2rCW1SDbiF9BXi+rLFdizvZ+s69FBg4BqhJpzTOQ51D3CMDvfYYYhDvHIo68BYdiZ+DqLLJ7Q35JcF/p6YbJwNAtUE1TQtLoOU2COjMO6NE2evMMMAzBLxqsvBrAa7/IB5+LUONlQrYGurMEErCB/AKFkjkUyN8imagX0oCtrMAErOMQHNhcmpnkj/t1xhZ6qbwIuw8CVdIJorvpAR9c2gSLDQokca58bIJusFsgPHV3bBKzgHT7QOD+8WjPTkBf5KV0T4IZt8kExoXjOfntW7z/fyIe8lJ4plDRMcmF8mv9WquzbE/HIQ+U3jbKGgVB8jYUnHx75xy32Iw7xVF4TsYKDfOCRs0MvWcvIArs0Nco6H/eLuwwHt4ai4ornmMc69mE/lcdkuGEbfFDHK1ZwgA/qeKZu2JHYYP8AO4gg4ZYduUsAAAAASUVORK5CYII=")
        //new TemplateData("iVBORw0KGgoAAAANSUhEUgAAAEgAAAAgCAIAAAA+KKknAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAARHSURBVFhH3Vd9bBRFFN/ZlmuhvaOf0IS7Hldp0RD6HT0qBCxtTMSIPUwFNTGEfzTRGE+TiqQpgcSkJhwGDIEA0X+IQKClYDSmVDSxVfpFC5Kq5evahkh7LUpbe3cte/5mZ27duxJQurs1/PLLZd6bN7Pz5r03M0dsNScEQSAhKSk4YQkEiBCCOHNIRBycm+CPNXHZcBDb9joiSc6hvi09rYlTAa7WAjfnWnYUl/8RN4/LxgKO1VuCE7tazuQPD3CddvDkrfliSSEXjIUI30xSaP6kXxSI5jRPBjH/rBCOoapCIYGQUEhzalSwDwMaMRIicJLoAfqJiI00jCLbVOqXWq0R6Q/mnw3SVHwkITsGF3WCfjM/CCJyRatLeTrkhKTzG0/UGG2oC0Nbqr7FWbI4/bz7We/29YyXt65z5dqibGZOo2usxJG220Wv7E2ft9hrToPLPvqqrpu+DVx51ssfPnf2rWdkw5lCPhUZ9YAyeZjOxWkpCabvegdbrvmiuijvNeThGCtPZhwWmuNNMaI1OfoB6S59/M1VS9CVvcDs3fFC7+Bo2afnoKcR3lCYYYlnZkc7+qoaulibDWn1jgzc/mtjUSY0Y4Gp6i8vsvgjYvLDQy/Ik6vYcLH/9zv+lVlpNOVUek9TT1V9F1YGl+zVDWV7v4XSlWs9+MqTmGXTZ81QHu3wwoEjr6/go2hohGUZliJbMnphg+E7n8/FKPQaXWPIwHdPdsA3Gpmd62tfzOcd98KGApspVjzW6aV5KwhVp7rgdr412ZVnYwaAbyxAd0GeGZawf8qRClEk3HN9gDfoNP541ef8+Js9534N3pU2FtnPvl3K9AoUS+TtyHjw/DWfounsH0mMi3U6UpkIDI76ld4bvvHglGRLmof2rL08PE2/5NScQQQQuiObS7hWhZKsNHP8HC78d8g1pmPIMPc/hTSd+77/DYWxwBzPNaohLVeHRv2TihgmM5CVCsK9jtQEpCLTzFrE/g2QZrgbnFnpXBaEwswUbMRP12nJTQdSF78dfSP4Dd9jOoFNrmJtRUHjO2sV8aXCTOzx1z/fRPvWnxNYNNIPRzzrPdHZh5qpLLIzDcYib7sHbtdf6GcGwNOPpUOPdkW+bd3yRd7hcU9jD0Ri31qf6h/b13oq586QvBYtsT97xYGlTi6EUesqeLnYztrwpPp0N12oDHfZE2+szsZthtor/6QJGlSap7JYuceOtXur6i6wNjNuuzGMTIbD0OCwdR9vZ0cosX8gO9amm2M50Y5pBXe57Nj14VcPN3OVCux1D8hx1RoRz1Ktqaw5Ss9o9FtRYzJEKWX+r0/F+2N3Y8/SbQ2vHfqBy5HQ+VQEVLtoJI0+7g0jUhH/n6UY9UWuHURBiviagaQRG42J602kL2LNcWl+RuTnjCNxvH8SK0icDJTeurLQP8YWpAkuJWW0p1inxBguGwvieI869qhBEP4GgyulNRPb7m0AAAAASUVORK5CYII=")
        //new TemplateData("iVBORw0KGgoAAAANSUhEUgAAAJIAAAAeCAYAAADHCUctAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAScSURBVHhe7Zo9SFtRFMf/7RwQDUpSwUXEJUMVWozURerQFl3UTQcX4S1OgtCluAgVJ5dAFwe7tV0qrUPFRYmiUBeXoC5Cm2BJSsG9ved+vHffR/Jic0loc38Q8vJyfe++c//n3HNOvHd5XfwNi6VB7st3i6UhrJAsRrBCshjBCsliBCskixGskCxGiC7/K3msrBVwID+GSeHNxjNk5Ke2IdYugOMsYL5ffmgjYoT0/wrm/P0WFo/vuPA17VLGl82PeHVNx43a7QLby4fI/UP2t1ubMZKYWFrAznSCHZewuJln0mofGoxIynOAsekpvM4mxWnF1S5GcyVg5AnyMwPypN/b0kfvMPnhVn5X5TqScmAsER1RqtyjbxA7S934LOfspw7vr8suXmQKz82zlwuf0yiToUTZLIjPhnexRXNoMCINYH7jCRx2dPBhH18q4qyAGY0MQobSDKBTZMZYxzjyGwv89WaErvMRK0dBX6bF2eKGI2Op8eT9udwWRt9fyHFhfPfgC0ZzFvcivOuZ2kKSGH5EUQnInenzIhFdYkjOXc0f1wVM6vPvf8a+EzYV4pbjAyLS7abG53LvAmvQPAxsbWxhnBR7v8WrtV2ci5MsByHPS2B1TvM2HyXsMWPo0Sczo0R54l6HKB/tcw+naKV7XDI7KwRxfIjtK3HOT/gezSDZI4SE40vtOUjAfrEmsw+FYHzj4qHn9j/TAJ7zLfUWe4XWbKgxQmJ7/TLzeN/LE4sL8yI3NyDvYuGZEtmx6XFMdIkhYVKYDy3wAIZ4pCjhzBVGGV9PKYQn8HQwLIjMEImYeeOnqJwk6h5NoLMDY/KwXYgRkhZa3Vf0FpDMjmO1jx2w6CD2+L9bxHSv8ObzGyWLCkq8EkogHSVKtWjXv1DkJyytwGDVxqqWFyI6EI5jKOeo/Kgd9ru6DeU2Bvn5S/Sa+jqQ5icUItfzontU0l8nlJRrO0Uw8W42BoUkk2tJ9FYTT/GbMEimR0azOKHECa0FlG/koj7o9ldjy1TNsbzxpYruKqm+C1KMqhqWO4VILVqHISHRw8nkmhmJJ8DBaqQuyih+p/cEUp38BKMLKdoyWSJZjKpIqnp/q/ByutVxVWkx+3wiJyP7zNbIG+vg6kQ0PQPtgFZjREiqqnIcYaTMzJSbL0VXU1WoFLBHRurrxbBrbFVOR1ck52ciCjovqlWHtfFyMTN4FaZeaMTkeVUJO4+KdmO9jajRPI0LqZLHOu3PzEO80pzlS3ODPAmu3tugilD/jnntW2r2hVsGKpGnHpMuTOqnUHXov3d9qGrv4LTA7mwCseXwXIXNJ1ieq2p02+2RqShO6FUqocaHnSc52Mvt6muRsG1T5UjB5yEb8Twq2Gmn5irPr8LrQz8f8b+5w47ylz/ayi5qZ+1Or9d91b/Xu86PUXR/nyKir6Oov5ur36NG0u/rIseMJWLtUusaJBz9WeU2B++awa6++j2Qo29lvnkz5HfeeG8ers2CHXT3WcLbrXsd/Z4xtOCf/+tcZMs/hcGqzdLOWCFZjGCFZDFCC3Iky/+IjUgWI1ghWYxghWQxghWSxQDAH9NBcSUbvOwfAAAAAElFTkSuQmCC")
        //new TemplateData("iVBORw0KGgoAAAANSUhEUgAAACsAAAArCAYAAADhXXHAAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAGPSURBVFhH7ZjLSgNBEEWrOwmZiI+V4CaPhYroJ4ggguCvxLWP+AiCC38kbtWIor8g4kL0A4IbdyKCaBLzsDrUwnEqs0t3V8iBy/Sd1Z1LMVOJyp7cdLvQAZ9RoEHrJGjfgxpMxnaniZG7PSdCGi9ikNYsc9dTyWpW4VWKRDWrssdX5hhhc3kWSmsL5OxReXiBg9tncmGwWczaT05gcpCG5D3rEi4PSlSzKnd0aXJHmB5Pw8xEQM4eb19NeP34Jhemb1gfkbYbyEHlyvwYpJMagmSCnD0a7Q7Uf9rkwmDYKhu2uDIHpfVFcvao3Nfg8PqJXJj4mXUFlwUVu3W5gsti5N8XjMtCEjUGKr9/YY4RJoMUTGVS5Ozx2WjBO37FOFR+jw/rI6Of4oOSypdiZnbMwczWY2a2sHvOhi2uzsPOxhI5e5ze1aBcfSQXZohm1gVcDpK0ZjEyp97juOBfjj9She0zPEUx+2w6ZX+fbbZi9tnCFh/WR0Z/zA1K+DZg7noqac3KQVizWkC5CYyZCeAXnK6h/6KX5CIAAAAASUVORK5CYII=")
        public static readonly TemplateData[] pascoStart =
        {
            new TemplateData("iVBORw0KGgoAAAANSUhEUgAAADQAAAA3CAYAAABD7GlFAAAFiElEQVRoge2ZXWwUVRTHfzuzX93PAkrZsptgAsFK1dgSSkNifDImViW2PpIuLxgx8MQDmKxGiJZXSzTIS9sHn1qDaH0h1JiobYrsBoGkNSqYbt0iKB/ttju7250x93bbgBjT3RnKptl/crMzt/feOf/5n3vuOVPboUOHDFYR7A9jUcMw0HUd98wM3vQcumFgA9LeGua8XhRFWWo2m83SZ9vFw62CWEuQ2Dz2C+G/ruMjh+pzYnPaUVwOjJzO3XmVq7YaxjY/gRYM4nA4LCVmmUKCzNafrrBtbAzF70DxOVA8NSg1ThT3QlNdLjxuF/WKyvar1/j+msKVbQ243W7sdrslpCxRSNWytH73A3W3b6EEnNi8dhSP/QEyirvYXC58O9by4nSWTUPDfPnsMzhqg7jEGEUxZYu52UVlJJk7t6QqyyFjc7lknyu8nqdea6Xt4iWmp6fJ5/OYfcGKWKDcJja+cDOpjLc0MorLLX8djwdpeOk5dlwZI51OL5Eqt5lSSASAxvHxkpVZJGPoOoWshhpw8/wGL8aNG2QyGQqFwsorJNTZMv6rDABmyOhaFj2Xxb15PS2/J5mdnTWlUtkKCUIiNC9EM3NkdE3DMOZp9KmSUC6XK1uhsqOccDe/Lf+foblUMno2K+99PgfBW7fRAgF8Pt/KEhIZgDg0rSKjZ3OggprNkTER7cp3OZHOiAzAIjIF2Z9H1wumgkL5B6vYgC6HdWS0HEZuHgz70gYvB2UrNONxy9zMMjJaDn0uR0Y1d9aXrVC6poY7eYV6q8hk8syk55kKOalX1ZVXSFVVfsONTVEtIaOn81xClfmc0+ks16zyD1aBy5siaFf/Nk9mdp7C3Rxng4GlzHvFD1aRFWt+P99O6+jTWVNkhDrfqE60gB+v1/voFBLF2cWtW/j53CUKd+bKIzObZ0IzGFq3hmAwiMfjke78SJJTUZAJnx9oeJLLp4fRkn+WrMxExuDjunWyHvL7/aZrItMFnnib9mCAzxob2HXuMi/U+2WiKXKz/wsAYs8MKQ6GQmtw1Qapra2V+0eQMWOT6RJcqCR8PhAIcL6xgfM3b7JraJyn/XaZm4l0RmQA4tAU54wIzSKanV27jozfJ91M5G3C1YQLmy3DLSnBhRGLxmh1dQz5/Xw1N8fau9PYZTqjgOEkY3czVeeUbiXUeMzjkUFA3AulKVbApgiZZlPEIinhMkIxYWguGCQ7Py9zM2GoGBNSVTlOjBG/IkSb/Y5wLyz9jEUxnAtjF0ktB1baYN2rqRBYrtCjxqojVHW5SkdVoUpHVaFKx6pTyNR/H1aktbzNB6+Hlz2+pOQ00v4h77+8oXh3na/fe4fPkw/nTZeLkvaQGJo4tZcTI5IdXbt3MiBvHjJK+PBoWfnQeqCHfU3iKsGpvSeQNAXpo21ITROn2HtihEh7F0fbFlReejmtB+jaDjQ1weC7HEntpmdhMRKJBKSWb0dpCmHQtK+Hnn3A1CCxI8PI2a0HeSUVI9qdlNdd7WGGB6DjrWYuxKIMLLllK7ubLxCLDpCMdNC1v53w8ABJAzaEUsSi3SSJ0NEVYrA4r/VgL69yetkKlRzlEp9GiUajxOLN7O+IyL7IxhChtmP09vbS+2YTofowEKaeOKP37rHIRkJTfyC7kqPEqSdc/NNUfHSh/1/zRn5MlGRfSVGOYjEm2kT/GVLNLUTE9WSKqcEYnZ2dC+2jYQxjkhTNtETuWWNiklRoo5xjRFpoJsXkA2vfP2/n9qb7nmtplLsfI3wRP87+N0Y53N/NmYN99PUV3/ZgjMP9E/R/Euf4sT7aRGf8JJ3dC3OO9cke4ic7mXhg3Qn6z6ToK86Lx+MlWWXbs2fPqkoVqqlPpaOqUKWjqlClo6pQpaOqUKXjH5oBCTYA9MWmAAAAAElFTkSuQmCC"),
            new TemplateData("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"),
            new TemplateData("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"),
            new TemplateData("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"),
            new TemplateData("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")
        };
        public static readonly TemplateData[] pascoStop =
        {
            new TemplateData("iVBORw0KGgoAAAANSUhEUgAAACwAAAA0CAYAAADxAdr3AAADvklEQVRoge2YTWsTQRzGn31LNbZFTzZp4odIK20vfgFfqKTeWho9iD3EW1EPLYhg9WijYPUQ4tUKCv0CejCWNPkEgpjErYiRitK87GZWZmZTU1CYrTPFQB4Ydvbf2ZnfPvnP7Ey1xcVFDz0k0/N6ihemcMNmC8O1byCEgHgeNEkAOyeOgxw9AsMwxDhEHR76WsOZ12+gDRjQQjovFi0mNMuAZhrQTXo1WZ1f99d1w+yKm3BrP/Cs8hM7sSgGBwfFgIVaAaAvRmH1Yyb0IwYHHzChhywOHTKhW517C7rFrxqLhfZiPB5isYbxGd8/fsDu7nFxYFGH24QwV2XBghB4roN6fRfNZhOiHLpQKzCLeRpIgiVOC57r0m6FYRF0lWA5KwmWtDgwJfb8IiJxhxmwRFjHAXHbQYZnEnaYTTq6GkiCbTsOd9jvW43DdImSBUtL2w0yPFMgh+k6Kw3Wd7gzvihHsElnSoQ9tJSQBEtaFFj1pKMOy4I9HIdNabCELWuKJx1PCUmwjstSovOlU+awNFh2rzCH0ZUSUmBbrj/p+D5YicN0PysLltVVOrx/lZAA2+zksMrND0sJSbA0JVokyPBMwR2WBEsabXitNjzzd/9CwKJvpmkaO4PRYw09KbB9gOuyLSKrt929mOfHSKfutPdfW4TDNgmMQZ31LSphh2vDQ3hqb+PH+y9oNOps/RQfRveL5Y8K5iyFpf2GTVO+w+5ACD8jJ1GvD8OhP32AifInUVdpCVsWwuGw8HPCDuu6zjoO0nkQKVkl/gf13r+qeg24nxKq1XdYtfoOq1bfYdUS3vx0K568i9tnR/hN6QmuPHwnGevvCu5wPImFsS0sX36BCr2fSiM9mUemmsTKtI1bmbwiVK4DOQzb5rBU+QwyiCO5cg4jI0A2exWlJ5eRQRrZqwnW5PPGMm69qLCXXZmOwk4kkOiOB5DefQgUKuV13CyMI5vN8pKehOeVsf5oA9ulNaRSKay+jSF5HlhLpZBKrcE+N41J9jzgJYACiy9ha2wByViw8Q/mcH4VKf+Xn7q+gpl4Huv7GsQQhY1N3hiF0gWMxv0/lQrgj1awWQQuxlhVocPxGaRn4l2xCCKjHthM2ItVYSOCUVafxHjCRrXst0mM+27HMTEG2FXVDpef4+XEPeRyEX5ffIx5ZtkmitE7yOWuofh4HquvgFwuh2sAtjeWsDcVi8DprvjNcrDhtbm5ucNbiE9dwv2Ln3DjwdsDd3G4Hw6eN/92eJ2dne2pT13PfZr7mx/V6jmHfwEIGH2LAjM/WwAAAABJRU5ErkJggg=="),
            new TemplateData("iVBORw0KGgoAAAANSUhEUgAAAC4AAAA1CAYAAAA+qNlvAAAEV0lEQVRoge2YTU8bRxjH//tik7oEpaeya8yHMLTCHPIF4jRIFicgijlYPQTSA1JKlRK1jaC5NSWHiEq4oscYoQR/gfZgorb4E1Sq6rdFValSJcLYXu9WM7trdiNq727Hi1B4pNHOPp6X3/znmdkZc0tLSzrOoYm+KjWaGDr8G5qmQdN1cIxgXr53Bdo7lyAIQm8GXfcu+OW/DnH1x5/ADQjgwryRQiSJ4EICOFEAL5KnSPPG05nnBdHmF6EevsJW+TVejsgYHBzsDe6ZGgAZLIHm3xXBXxKMAQyI4MMhAz4sgg9Z7yHwIePJUV+44zP8Yeo7Fg7wzx+/4+joijtwP4q3NY2qzAoamgZdbaFeP0Kj0YAbJt4zNajkRngwgtZaTeiqSpp1BQ2/ihOjMc0IWmsa4IRcN1Mv86c4BWcI3WpBU9ue+velOF2cZPdgBN1utQzFzbb7qzjZ2lhBk9RWPfXvW3GyTzODNhW3WNww+V+cIkPowEOFEbTWJOBBLU6iOCvoYBUXmUFrdDsMaHEaocIIuqXSULG+nH1XnBk0fQ8gxmELFSbQTdVcnMY5vK+Kk/M0K2iaD0Jx567CALphxXgQhywaKoygSag0NU/9/z/FGUFrx23ozTZ08aT9nuCeqYnaHEfviOS6RW4u9JyhqvRoSvNttePTTZ9m5Vtt57OpGdANDcIgT9t2Y74UPxy6jO9qCl799ieOj+t0/3V/0+fNFDIJQJUm0KTdiCj2T3F1IIzX0vuo14fQIiHhckH9lxGVSYqEQohEIq7q+FKc53nagdtOvFpfd5WzNt9fzrO2cwt+ESpB24XiQduF4kHbW6Z4LIXVL65huOMoYmP+MV6Q7MRtbI7/ivnHL5iCvmneD1kUegz79+exXbacE7i9mkL1s22Uu9dmZt4Vj0oYLj5HrmSvt4f1ZWNQa5k4gDg2NzPGTKTXsYcEFrIZxGnZA+RXlk8GnVhAdryGvJxE0pzC4kYa63vdMbzHeKWGg3gGC4lTfitvY3mjSHpGOp1GmkLHkFrLQM6vGL6NGpJfLsBRPZ6E/Dxt/L6Sh5xZQyrWA9x+QXWVSjl8+nkeUiaLbNZMqymMdMqQZu11opCHi3iWKxnvhR3klTjGJ2zllTx2CiftPysOQ4525/B1kUA5h+Vbuc5rYvF7fLVYxa1vT5nfWBSSszKqCiDbXUrVsTYqNQUfRWO0LDvFT0mFnTwUKYqYdRNyzFAFNccMxMgycf4VYatLUlSSUKuUuvbpPcYTi/h6etTpmkpCqlVQshzyCE5KVKEoY7hh1UlMISnt4xf75EhJTFlBPzqNG2MKmYSuxs3NzXneyEenH+LBdVsAKLu4d/epCT6K6YcPYPy8jyc3H6GASdzZ+hhjRmHs3ruLp9YoJ+9g64MaduXrsJrcf3ITjwo9wGdnZ8/2mz/5CX748GfMfdOD9A17yz75LM22fXoxbmZm5lweD89ecZ92EeNB27lV/F/3KcKfNz8hDwAAAABJRU5ErkJggg=="),
            new TemplateData("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"),
            new TemplateData("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"),
            new TemplateData("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")
        };
        public static readonly TemplateData[] hamburgerButton = { };
        public static readonly TemplateData[] exportData = { };

        public static readonly TemplateDictionary encodedTemplates = new TemplateDictionary()
        {
            { Template.SparkvueStart, pascoStart },
            { Template.SparkvueStop, pascoStop },
            { Template.HamburgerButton, hamburgerButton },
            { Template.ExportData, exportData },
        };
        #endregion

        #region MOUSE STUFF
        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;
        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, uint dwExtraInf);
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point lpPoint);

        public static void Click(Point location, Barrier barrier = null, Stopwatch stopwatch = null)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)location.X, (uint)location.Y, 0, 0);
            if (stopwatch != null) stopwatch.Start();
            else if (barrier != null) barrier.SignalAndWait(); // maybe the delay would be more accurate with these two lines below LEFTUP ???
            mouse_event(MOUSEEVENTF_LEFTUP, (uint)location.X, (uint)location.Y, 0, 0);
        }

        // convenience function
        public static void MoveToAndClick(Point location, Barrier barrier = null, Stopwatch stopwatch = null)
        {
            SetCursorPos(location.X, location.Y);
            Click(location, barrier, stopwatch);
        }
        #endregion

        #region KEYBOARD STUFF
        private const uint KEYEVENTF_KEYDOWN = 0x00;
        private const uint KEYEVENTF_EXTENDEDKEY = 0x01;
        private const uint KEYEVENTF_KEYUP = 0x02;
        private const uint VK_SHIFT = 0x10;
        private const uint VK_CTRL = 0x11;
        private const uint VK_RETURN = 0x0D;
        public const char CH_RETURN = ';'; // arbitrary char representation; just need a free char that's invalid for file names and doesn't use Shift
        [DllImport("user32.dll")]
        private static extern short VkKeyScan(char ch);
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        [StructLayout(LayoutKind.Explicit)]
        private struct VK_Helper
        {
            [FieldOffset(0)] public short Value;
            [FieldOffset(0)] public byte Low;
            [FieldOffset(1)] public byte High;
        }

        private static void HoldKey(byte virtualKeyCode)
        {
            keybd_event(virtualKeyCode, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYDOWN, 0);
        }

        private static void ReleaseKey(byte virtualKeyCode)
        {
            keybd_event(virtualKeyCode, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        public static void PressKey(char key)
        {
            VK_Helper helper = new VK_Helper { Value = VkKeyScan(key) };

            byte virtualKeyCode = key == CH_RETURN ? (byte)VK_RETURN : helper.Low;
            byte shiftState = helper.High;
            bool holdingShift = (shiftState & 1) != 0;

            if (holdingShift) HoldKey((byte)VK_SHIFT);

            HoldKey(virtualKeyCode);
            ReleaseKey(virtualKeyCode);

            if (holdingShift) ReleaseKey((byte)VK_SHIFT);
        }

        // for convenience/explicitness
        public static void PressEnter()
        {
            PressKey(CH_RETURN);
        }

        public static void PressCaptureScreenHotkey()
        {
            HoldKey((byte)VK_CTRL);
            PressEnter();
            ReleaseKey((byte)VK_CTRL);
        }
        #endregion

        public static void CalibrateTemplate(Template template)
        {
            // this should need to be run ONCE PER COMPUTER.
            // basically, if values aren't found wherever persistent storage is dealt with, run through this.
            // there should also be a way for user to call this function again manually (some button somewhere).

            // Todo: save to persistent storage -> { [templId]: { detected: [minValDet], notDetected: [minValNotDet] } }
            // Prompt user:
            //      Is [template] on screen?
            //          - DetectTarget([template]) -> store { detected: minVal }
            //      Is [template] not on screen?
            //          - DetectTarget([template]) -> store { notDetected: minVal }
        }

        #region TARGET DETECTION
        private static Mat CaptureScreen(int index)
        {
            Rectangle bounds = screens[index].Bounds;

            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height)) // new Bitmap
            {
                using (Graphics g = Graphics.FromImage(bitmap)) // take the screenshot
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                }

                return bitmap.ToMatBgr(); // convert RGBA Bitmap to BGR Mat and return
            }
        }

        public static Target DetectTargetOnScreen(Mat template, double threshold, int screen = 0)
        {
            Mat screenshot = CaptureScreen(screen);
            Mat result = new Mat(); // ref to hold MatchTemplate() result
            CvInvoke.MatchTemplate(screenshot, template, result, TemplateMatchingType.Sqdiff); // run dumb algo to find best match of template in screenshot

            // exempt from naming conventions (every person in every language will name these as they are)
            double minVal = 0; // essentially, 'arbitrary' confidence value for detection accuracy
            double maxVal = 0; // used for other TemplateMatchingTypes
            Point minLoc = new Point(); // 'best guess' top-left corner location of target
            Point maxLoc = new Point(); // used for other TemplateMatchingTypes
            CvInvoke.MinMaxLoc(result, ref minVal, ref maxVal, ref minLoc, ref maxLoc);

            return new Target(minLoc, minVal, threshold, screen);
        }

        // detect target on all screens
        public static Target DetectTarget(Template template, bool onScreen = true) // using TM_SQDIFF
        {
            Target bestGuess = new Target
            {
                value = int.MaxValue // guarantee replacement
            };

            for (int templIdx = 0; templIdx < encodedTemplates[template].Length; templIdx++)
            {
                Mat _template = encodedTemplates[template, templIdx];
                int threshold = encodedTemplates[template, templIdx, onScreen];

                for (int idx = 0; idx < screens.Length; idx++)
                {
                    Target currGuess = DetectTargetOnScreen(_template, threshold, idx);

                    if (currGuess.value < bestGuess.value) bestGuess = currGuess;
                }
            }

            return bestGuess;
        }
        #endregion
    }
}
