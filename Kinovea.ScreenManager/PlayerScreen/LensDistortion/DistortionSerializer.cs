﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;

namespace Kinovea.ScreenManager
{
    public class DistortionSerializer
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Serialize(XmlWriter w, DistortionParameters p)
        {
            if (p == null)
                return;

            w.WriteStartElement("CameraCalibration");

            Action<string, double> write = (element, value) => w.WriteElementString(element, string.Format(CultureInfo.InvariantCulture, "{0}", value));

            write("Fx", p.Fx);
            write("Fy", p.Fy);
            write("Cx", p.Cx);
            write("Cy", p.Cy);
            
            write("K1", p.K1);
            write("K2", p.K2);
            write("K3", p.K3);
            write("P1", p.P1);
            write("P2", p.P2);

            w.WriteEndElement();
        }

        public static DistortionParameters Deserialize(XmlReader r)
        {
            r.ReadStartElement();

            double k1 = 0;
            double k2 = 0;
            double k3 = 0;
            double p1 = 0;
            double p2 = 0;
            double fx = 1;
            double fy = 1;
            double cx = 0;
            double cy = 0;

            while (r.NodeType == XmlNodeType.Element)
            {
                switch (r.Name)
                {
                    case "Fx":
                        fx = r.ReadElementContentAsDouble();
                        break;
                    case "Fy":
                        fy = r.ReadElementContentAsDouble();
                        break;
                    case "Cx":
                        cx = r.ReadElementContentAsDouble();
                        break;
                    case "Cy":
                        cy = r.ReadElementContentAsDouble();
                        break;
                    case "K1":
                        k1 = r.ReadElementContentAsDouble();
                        break;
                    case "K2":
                        k2 = r.ReadElementContentAsDouble();
                        break;
                    case "K3":
                        k3 = r.ReadElementContentAsDouble();
                        break;
                    case "P1":
                        p1 = r.ReadElementContentAsDouble();
                        break;
                    case "P2":
                        p2 = r.ReadElementContentAsDouble();
                        break;
                    default:
                        string unparsed = r.ReadOuterXml();
                        log.DebugFormat("Unparsed content in Camera calibration: {0}", unparsed);
                        break;
                }
            }

            r.ReadEndElement();

            // Due to numeric instability, we drop k3.
            k3 = 0;

            DistortionParameters parameters = new DistortionParameters(k1, k2, k3, p1, p2, fx, fy, cx, cy);
            return parameters;
        }

    }
}
