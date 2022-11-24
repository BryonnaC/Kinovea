using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Analysistem.Utils;
using Emgu.CV;
using static Analysistem.Utils.FakeUser;

namespace Analysistem
{
    // wanted to put CsvFile here but it's definitely large enough to warrant its own file

    static class FileHandler
    {
        private static readonly Dictionary<string, Mat> encodedTemplates = new Dictionary<string, Mat>()
        {
            { "hamburger", LoadMat("iVBORw0KGgoAAAANSUhEUgAAAJIAAAAeCAYAAADHCUctAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAScSURBVHhe7Zo9SFtRFMf/7RwQDUpSwUXEJUMVWozURerQFl3UTQcX4S1OgtCluAgVJ5dAFwe7tV0qrUPFRYmiUBeXoC5Cm2BJSsG9ved+vHffR/Jic0loc38Q8vJyfe++c//n3HNOvHd5XfwNi6VB7st3i6UhrJAsRrBCshjBCsliBCskixGskCxGiC7/K3msrBVwID+GSeHNxjNk5Ke2IdYugOMsYL5ffmgjYoT0/wrm/P0WFo/vuPA17VLGl82PeHVNx43a7QLby4fI/UP2t1ubMZKYWFrAznSCHZewuJln0mofGoxIynOAsekpvM4mxWnF1S5GcyVg5AnyMwPypN/b0kfvMPnhVn5X5TqScmAsER1RqtyjbxA7S934LOfspw7vr8suXmQKz82zlwuf0yiToUTZLIjPhnexRXNoMCINYH7jCRx2dPBhH18q4qyAGY0MQobSDKBTZMZYxzjyGwv89WaErvMRK0dBX6bF2eKGI2Op8eT9udwWRt9fyHFhfPfgC0ZzFvcivOuZ2kKSGH5EUQnInenzIhFdYkjOXc0f1wVM6vPvf8a+EzYV4pbjAyLS7abG53LvAmvQPAxsbWxhnBR7v8WrtV2ci5MsByHPS2B1TvM2HyXsMWPo0Sczo0R54l6HKB/tcw+naKV7XDI7KwRxfIjtK3HOT/gezSDZI4SE40vtOUjAfrEmsw+FYHzj4qHn9j/TAJ7zLfUWe4XWbKgxQmJ7/TLzeN/LE4sL8yI3NyDvYuGZEtmx6XFMdIkhYVKYDy3wAIZ4pCjhzBVGGV9PKYQn8HQwLIjMEImYeeOnqJwk6h5NoLMDY/KwXYgRkhZa3Vf0FpDMjmO1jx2w6CD2+L9bxHSv8ObzGyWLCkq8EkogHSVKtWjXv1DkJyytwGDVxqqWFyI6EI5jKOeo/Kgd9ru6DeU2Bvn5S/Sa+jqQ5icUItfzontU0l8nlJRrO0Uw8W42BoUkk2tJ9FYTT/GbMEimR0azOKHECa0FlG/koj7o9ldjy1TNsbzxpYruKqm+C1KMqhqWO4VILVqHISHRw8nkmhmJJ8DBaqQuyih+p/cEUp38BKMLKdoyWSJZjKpIqnp/q/ByutVxVWkx+3wiJyP7zNbIG+vg6kQ0PQPtgFZjREiqqnIcYaTMzJSbL0VXU1WoFLBHRurrxbBrbFVOR1ck52ciCjovqlWHtfFyMTN4FaZeaMTkeVUJO4+KdmO9jajRPI0LqZLHOu3PzEO80pzlS3ODPAmu3tugilD/jnntW2r2hVsGKpGnHpMuTOqnUHXov3d9qGrv4LTA7mwCseXwXIXNJ1ieq2p02+2RqShO6FUqocaHnSc52Mvt6muRsG1T5UjB5yEb8Twq2Gmn5irPr8LrQz8f8b+5w47ylz/ayi5qZ+1Or9d91b/Xu86PUXR/nyKir6Oov5ur36NG0u/rIseMJWLtUusaJBz9WeU2B++awa6++j2Qo29lvnkz5HfeeG8ers2CHXT3WcLbrXsd/Z4xtOCf/+tcZMs/hcGqzdLOWCFZjGCFZDFCC3Iky/+IjUgWI1ghWYxghWQxghWSxQDAH9NBcSUbvOwfAAAAAElFTkSuQmCC") },
            { "exportData", LoadMat("iVBORw0KGgoAAAANSUhEUgAAACsAAAArCAYAAADhXXHAAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAGPSURBVFhH7ZjLSgNBEEWrOwmZiI+V4CaPhYroJ4ggguCvxLWP+AiCC38kbtWIor8g4kL0A4IbdyKCaBLzsDrUwnEqs0t3V8iBy/Sd1Z1LMVOJyp7cdLvQAZ9RoEHrJGjfgxpMxnaniZG7PSdCGi9ikNYsc9dTyWpW4VWKRDWrssdX5hhhc3kWSmsL5OxReXiBg9tncmGwWczaT05gcpCG5D3rEi4PSlSzKnd0aXJHmB5Pw8xEQM4eb19NeP34Jhemb1gfkbYbyEHlyvwYpJMagmSCnD0a7Q7Uf9rkwmDYKhu2uDIHpfVFcvao3Nfg8PqJXJj4mXUFlwUVu3W5gsti5N8XjMtCEjUGKr9/YY4RJoMUTGVS5Ozx2WjBO37FOFR+jw/rI6Of4oOSypdiZnbMwczWY2a2sHvOhi2uzsPOxhI5e5ze1aBcfSQXZohm1gVcDpK0ZjEyp97juOBfjj9She0zPEUx+2w6ZX+fbbZi9tnCFh/WR0Z/zA1K+DZg7noqac3KQVizWkC5CYyZCeAXnK6h/6KX5CIAAAAASUVORK5CYII=") },
        };

        public static Data ExportSparkvue()
        {
            const int timeToOpenBurger = 250; // milliseconds
            Target hamburgerTarget = DetectTarget(encodedTemplates["hamburger"]);

            // cache original mouse position
            GetCursorPos(out Point originalPos);

            MoveToAndClick(hamburgerTarget.location);
            
            Thread.Sleep(timeToOpenBurger);

            Target exportTarget = DetectTarget(encodedTemplates["exportData"]);
            MoveToAndClick(exportTarget.location);

            // return to original mouse position
            SetCursorPos(originalPos.X, originalPos.Y);

            string fileName = $"force {DateTime.Now.GetTimestamp()}";
            
            foreach (char c in fileName) PressKey(c); // type out the file name
            PressEnter(); // save the file

            return new Data(new Target[] { hamburgerTarget, exportTarget }, 0, fileName);
        }

        public static void ExportKinovea()
        {
            // TODO: auto export the Kinovea stuff
        }

        public static string[] GetPaths()
        {
            // TODO: dynamically acquire paths for the CSVs (might not need this?)
            return new string[0];
        }

        /* just chuck anything that has to do with paths or the filesystem here */
    }
}
