using Emgu.CV.CvEnum;
using Emgu.CV;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Analysistem.Utils
{
    public enum Template
    {
        SparkvueStart,
        SparkvueStop,
        HamburgerButton,
        ExportData,
    }

    public struct TemplateData
    {
        public Mat mat;
        public Dictionary<bool, int> thresholds;

        public TemplateData(string base64String, int? onScreenThreshold = null, int? offScreenThreshold = null)
        {
            mat = LoadMat(base64String);
            thresholds = new Dictionary<bool, int>()
            {
                { true, onScreenThreshold is int valueOn ? valueOn : 0 },
                { false, offScreenThreshold is int valueOff ? valueOff : 0 },
            };
        }

        public TemplateData(Mat mat, int? onScreenThreshold = null, int? offScreenThreshold = null)
        {
            this.mat = mat;
            thresholds = new Dictionary<bool, int>()
            {
                { true, onScreenThreshold is int valueOn ? valueOn : 0 },
                { false, offScreenThreshold is int valueOff ? valueOff : 0 },
            };
        }

        public static TemplateData Empty { get { return new TemplateData(new Mat()); } }

        public int this[bool onScreen]
        {
            get { return thresholds[onScreen]; }
            set 
            {
                var temp = thresholds[onScreen];
                thresholds[onScreen] = value;
                if (thresholds[true] > thresholds[false])
                {
                    thresholds[onScreen] = temp;
                    // TODO: some form of error handling,
                    // reaching this block means the user screwed up
                    // and claims the target is/isn't on the screen when it isn't/is
                }
            }
        }

        public static Mat LoadMat(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String); // base64 string to u8 bytes

            Bitmap bitmap;
            using (MemoryStream ms = new MemoryStream(bytes)) // stream across the bytes
            {
                bitmap = (Bitmap)Image.FromStream(ms); // convert bytes to Bitmap
            }

            return bitmap.ToMatBgr(); // convert RGBA Bitmap to BGR Mat and return
        }
    }

    public struct TemplateDictionary : IEnumerable<KeyValuePair<Template, TemplateData>>
    {
        public readonly Dictionary<Template, TemplateData> templates;

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)templates).GetEnumerator();
        }

        IEnumerator<KeyValuePair<Template, TemplateData>> IEnumerable<KeyValuePair<Template, TemplateData>>.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<Template, TemplateData>>)templates).GetEnumerator();
        }

        public void Add(Template template, TemplateData templateData)
        {
            templates.Add(template, templateData);
        }

        public Mat this[Template key]
        {
            get { return templates[key].mat; }
        }

        public int this[Template templateKey, bool thresholdKey]
        {
            get { return templates[templateKey][thresholdKey]; }
        }
    }

    static class TemplateHandler
    {
        // converts Bitmap (RGBA) to Mat (BGR)
        public static Mat ToMatBgr(this Bitmap bitmap)
        {
            Mat matBgr = new Mat();
            CvInvoke.CvtColor(bitmap.ToMat(), matBgr, ColorConversion.Rgba2Bgr);

            return matBgr;
        }  
    }
}

//public static readonly Dictionary<Template, Mat> encodedTemplates = new Dictionary<Template, Mat>()
//{
//    { Template.SparkvueStart, LoadMat("iVBORw0KGgoAAAANSUhEUgAAAEwAAAAlCAYAAADobA+5AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAP5SURBVGhD7ZlNT1NBFIbv36BUJXUDCTFYY0wAsQkbmjRu2ahsEERbqlAEAlRQoAViMNSaiHyoC03AKImLxujKRKKJ+pfGeYc717nTU3qh7YVJunh6pzNzznvO2+kHF6th5A2r4x2r4QEfeKRxZJM1T2ZZeHaMdSzcZt3Lvazn6XUWW42KK55jHuvYh/1UHpPxZNi5sTxrS0+wa9lb3Jwez2A/4hBP5TWRsoa1zqRZJHuTNMQriEceKr9pWAH+QNGUesEuzyV5w9EiA45HVORDXkrPFKzAfT7QCI2tsfb5QaLpykFe5Kd0TYAb9poP/tOUytfMLAnyQ0fXNgErkOQDhYO3Id1oNYGOrm0CLsNap9K8mWp9ZpUjKvRUfRNwDGsazVf8bXhUoAddtaDTjhUY5gNO28wE2VStga6swQS4YdssmHzFIhl/T5cEutBHHSYgDGsez5DN+AX09cLAyqd99vbrX4etL39YX75A7vULYVg4nSIb8Qvo64XBrPXCbxZb3itak6Tff6+6iYnNb+LFwZVatwKJbdYx30824hfQRx2S2NKeMAuGqPM6LsOI9eOQ2LAN41dqXRjWvdRLNuIX0FeLggEwYuXjvmteBWvk25WvScPlWu7zL3Z1bteJlevIIQ3CnsyHH06MRDfOCsS37Vs0dDN+AH3UoSINcZrV1kH6nXLC7DmYgRhc8Vyaj73qHhgGYJKcB64TpsxLhGH+/VgtRZQsTj1FasMSyjAdmA0DxWm156Rh1ItR1rBG/hA94RMGfdRBIZuTp61rdtdZcwx7XnDFqGA/4tRYmRMm6vtVw/Q1IAw7DZ9hemE6MAeNqE2WMgzPMY/9kuoZdm+LdT452W9J6KOOw+ia3VFOyo6Ycxlm75MNCzNKxMWyimF2nMRlmLYGhGHhmVGyEb+Avl6YjlfDYALMgCml4io2rCW1SDbiF9BXi+rLFdizvZ+s69FBg4BqhJpzTOQ51D3CMDvfYYYhDvHIo68BYdiZ+DqLLJ7Q35JcF/p6YbJwNAtUE1TQtLoOU2COjMO6NE2evMMMAzBLxqsvBrAa7/IB5+LUONlQrYGurMEErCB/AKFkjkUyN8imagX0oCtrMAErOMQHNhcmpnkj/t1xhZ6qbwIuw8CVdIJorvpAR9c2gSLDQokca58bIJusFsgPHV3bBKzgHT7QOD+8WjPTkBf5KV0T4IZt8kExoXjOfntW7z/fyIe8lJ4plDRMcmF8mv9WquzbE/HIQ+U3jbKGgVB8jYUnHx75xy32Iw7xVF4TsYKDfOCRs0MvWcvIArs0Nco6H/eLuwwHt4ai4ornmMc69mE/lcdkuGEbfFDHK1ZwgA/qeKZu2JHYYP8AO4gg4ZYduUsAAAAASUVORK5CYII=") },
//    { Template.SparkvueStop, LoadMat("iVBORw0KGgoAAAANSUhEUgAAAEgAAAAgCAIAAAA+KKknAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAARHSURBVFhH3Vd9bBRFFN/ZlmuhvaOf0IS7Hldp0RD6HT0qBCxtTMSIPUwFNTGEfzTRGE+TiqQpgcSkJhwGDIEA0X+IQKClYDSmVDSxVfpFC5Kq5evahkh7LUpbe3cte/5mZ27duxJQurs1/PLLZd6bN7Pz5r03M0dsNScEQSAhKSk4YQkEiBCCOHNIRBycm+CPNXHZcBDb9joiSc6hvi09rYlTAa7WAjfnWnYUl/8RN4/LxgKO1VuCE7tazuQPD3CddvDkrfliSSEXjIUI30xSaP6kXxSI5jRPBjH/rBCOoapCIYGQUEhzalSwDwMaMRIicJLoAfqJiI00jCLbVOqXWq0R6Q/mnw3SVHwkITsGF3WCfjM/CCJyRatLeTrkhKTzG0/UGG2oC0Nbqr7FWbI4/bz7We/29YyXt65z5dqibGZOo2usxJG220Wv7E2ft9hrToPLPvqqrpu+DVx51ssfPnf2rWdkw5lCPhUZ9YAyeZjOxWkpCabvegdbrvmiuijvNeThGCtPZhwWmuNNMaI1OfoB6S59/M1VS9CVvcDs3fFC7+Bo2afnoKcR3lCYYYlnZkc7+qoaulibDWn1jgzc/mtjUSY0Y4Gp6i8vsvgjYvLDQy/Ik6vYcLH/9zv+lVlpNOVUek9TT1V9F1YGl+zVDWV7v4XSlWs9+MqTmGXTZ81QHu3wwoEjr6/go2hohGUZliJbMnphg+E7n8/FKPQaXWPIwHdPdsA3Gpmd62tfzOcd98KGApspVjzW6aV5KwhVp7rgdr412ZVnYwaAbyxAd0GeGZawf8qRClEk3HN9gDfoNP541ef8+Js9534N3pU2FtnPvl3K9AoUS+TtyHjw/DWfounsH0mMi3U6UpkIDI76ld4bvvHglGRLmof2rL08PE2/5NScQQQQuiObS7hWhZKsNHP8HC78d8g1pmPIMPc/hTSd+77/DYWxwBzPNaohLVeHRv2TihgmM5CVCsK9jtQEpCLTzFrE/g2QZrgbnFnpXBaEwswUbMRP12nJTQdSF78dfSP4Dd9jOoFNrmJtRUHjO2sV8aXCTOzx1z/fRPvWnxNYNNIPRzzrPdHZh5qpLLIzDcYib7sHbtdf6GcGwNOPpUOPdkW+bd3yRd7hcU9jD0Ri31qf6h/b13oq586QvBYtsT97xYGlTi6EUesqeLnYztrwpPp0N12oDHfZE2+szsZthtor/6QJGlSap7JYuceOtXur6i6wNjNuuzGMTIbD0OCwdR9vZ0cosX8gO9amm2M50Y5pBXe57Nj14VcPN3OVCux1D8hx1RoRz1Ktqaw5Ss9o9FtRYzJEKWX+r0/F+2N3Y8/SbQ2vHfqBy5HQ+VQEVLtoJI0+7g0jUhH/n6UY9UWuHURBiviagaQRG42J602kL2LNcWl+RuTnjCNxvH8SK0icDJTeurLQP8YWpAkuJWW0p1inxBguGwvieI869qhBEP4GgyulNRPb7m0AAAAASUVORK5CYII=") },
//    { Template.HamburgerButton, LoadMat("iVBORw0KGgoAAAANSUhEUgAAAJIAAAAeCAYAAADHCUctAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAScSURBVHhe7Zo9SFtRFMf/7RwQDUpSwUXEJUMVWozURerQFl3UTQcX4S1OgtCluAgVJ5dAFwe7tV0qrUPFRYmiUBeXoC5Cm2BJSsG9ved+vHffR/Jic0loc38Q8vJyfe++c//n3HNOvHd5XfwNi6VB7st3i6UhrJAsRrBCshjBCsliBCskixGskCxGiC7/K3msrBVwID+GSeHNxjNk5Ke2IdYugOMsYL5ffmgjYoT0/wrm/P0WFo/vuPA17VLGl82PeHVNx43a7QLby4fI/UP2t1ubMZKYWFrAznSCHZewuJln0mofGoxIynOAsekpvM4mxWnF1S5GcyVg5AnyMwPypN/b0kfvMPnhVn5X5TqScmAsER1RqtyjbxA7S934LOfspw7vr8suXmQKz82zlwuf0yiToUTZLIjPhnexRXNoMCINYH7jCRx2dPBhH18q4qyAGY0MQobSDKBTZMZYxzjyGwv89WaErvMRK0dBX6bF2eKGI2Op8eT9udwWRt9fyHFhfPfgC0ZzFvcivOuZ2kKSGH5EUQnInenzIhFdYkjOXc0f1wVM6vPvf8a+EzYV4pbjAyLS7abG53LvAmvQPAxsbWxhnBR7v8WrtV2ci5MsByHPS2B1TvM2HyXsMWPo0Sczo0R54l6HKB/tcw+naKV7XDI7KwRxfIjtK3HOT/gezSDZI4SE40vtOUjAfrEmsw+FYHzj4qHn9j/TAJ7zLfUWe4XWbKgxQmJ7/TLzeN/LE4sL8yI3NyDvYuGZEtmx6XFMdIkhYVKYDy3wAIZ4pCjhzBVGGV9PKYQn8HQwLIjMEImYeeOnqJwk6h5NoLMDY/KwXYgRkhZa3Vf0FpDMjmO1jx2w6CD2+L9bxHSv8ObzGyWLCkq8EkogHSVKtWjXv1DkJyytwGDVxqqWFyI6EI5jKOeo/Kgd9ru6DeU2Bvn5S/Sa+jqQ5icUItfzontU0l8nlJRrO0Uw8W42BoUkk2tJ9FYTT/GbMEimR0azOKHECa0FlG/koj7o9ldjy1TNsbzxpYruKqm+C1KMqhqWO4VILVqHISHRw8nkmhmJJ8DBaqQuyih+p/cEUp38BKMLKdoyWSJZjKpIqnp/q/ByutVxVWkx+3wiJyP7zNbIG+vg6kQ0PQPtgFZjREiqqnIcYaTMzJSbL0VXU1WoFLBHRurrxbBrbFVOR1ck52ciCjovqlWHtfFyMTN4FaZeaMTkeVUJO4+KdmO9jajRPI0LqZLHOu3PzEO80pzlS3ODPAmu3tugilD/jnntW2r2hVsGKpGnHpMuTOqnUHXov3d9qGrv4LTA7mwCseXwXIXNJ1ieq2p02+2RqShO6FUqocaHnSc52Mvt6muRsG1T5UjB5yEb8Twq2Gmn5irPr8LrQz8f8b+5w47ylz/ayi5qZ+1Or9d91b/Xu86PUXR/nyKir6Oov5ur36NG0u/rIseMJWLtUusaJBz9WeU2B++awa6++j2Qo29lvnkz5HfeeG8ers2CHXT3WcLbrXsd/Z4xtOCf/+tcZMs/hcGqzdLOWCFZjGCFZDFCC3Iky/+IjUgWI1ghWYxghWQxghWSxQDAH9NBcSUbvOwfAAAAAElFTkSuQmCC") },
//    { Template.ExportData, LoadMat("iVBORw0KGgoAAAANSUhEUgAAACsAAAArCAYAAADhXXHAAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAGPSURBVFhH7ZjLSgNBEEWrOwmZiI+V4CaPhYroJ4ggguCvxLWP+AiCC38kbtWIor8g4kL0A4IbdyKCaBLzsDrUwnEqs0t3V8iBy/Sd1Z1LMVOJyp7cdLvQAZ9RoEHrJGjfgxpMxnaniZG7PSdCGi9ikNYsc9dTyWpW4VWKRDWrssdX5hhhc3kWSmsL5OxReXiBg9tncmGwWczaT05gcpCG5D3rEi4PSlSzKnd0aXJHmB5Pw8xEQM4eb19NeP34Jhemb1gfkbYbyEHlyvwYpJMagmSCnD0a7Q7Uf9rkwmDYKhu2uDIHpfVFcvao3Nfg8PqJXJj4mXUFlwUVu3W5gsti5N8XjMtCEjUGKr9/YY4RJoMUTGVS5Ozx2WjBO37FOFR+jw/rI6Of4oOSypdiZnbMwczWY2a2sHvOhi2uzsPOxhI5e5ze1aBcfSQXZohm1gVcDpK0ZjEyp97juOBfjj9She0zPEUx+2w6ZX+fbbZi9tnCFh/WR0Z/zA1K+DZg7noqac3KQVizWkC5CYyZCeAXnK6h/6KX5CIAAAAASUVORK5CYII=") },
//};
