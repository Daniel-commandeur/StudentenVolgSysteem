using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using StudentenVolgSysteem.Models;

namespace StudentenVolgSysteem.Documents
{
    public static class PDF
    {
        public static void Create(PdfViewModel pdfvm, Curriculum curriculum)
        {
            // CuriculumModel curriculum = db.Curiculums.Include("StudentId").Where(m => m.CuriculumId == pdfvm.curriculumId).FirstOrDefault();
            List<CurriculumTopic> curriculumTopics = curriculum.Topics.ToList();
            Student student = curriculum.Student;


            // TODO: Sort and group topics by first Certification
            SortedDictionary<string, List<Topic>> groupedTopics = new SortedDictionary<string, List<Topic>>();
            foreach (var topic in curriculumTopics)
            {
                string first = String.Empty;
                if (topic.Topic.Certificeringen.Count != 0)
                    first = topic.Topic.Certificeringen.First().Naam;

                if (!groupedTopics.ContainsKey(first))
                {
                    groupedTopics.Add(first, new List<Topic>());
                    groupedTopics[first].Add(topic.Topic);
                }
                else
                {
                    groupedTopics[first].Add(topic.Topic);
                }
            }

            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            var tf = new XTextFormatter(gfx);

            XStringFormat stringFormat = new XStringFormat();
            stringFormat.LineAlignment = XLineAlignment.Near;
            stringFormat.Alignment = XStringAlignment.Near;

            var basepath = AppDomain.CurrentDomain.BaseDirectory;
            var path = String.Concat(basepath, "App_Data\\logo_itvitae_learning_liggend.png");

            //var path = Server.MapPath(@"~/App_Data/logo_itvitae_learning_liggend.png");
            XImage logo = XImage.FromFile(path);
            gfx.DrawImage(logo, new XPoint(430, 5));

            //Fonts
            XFont Calibri20Bold = new XFont("calibri", 20, XFontStyle.Bold);
            XFont Calibri20BoldItalic = new XFont("calibri", 20, XFontStyle.BoldItalic);
            XFont LabelFont = new XFont("calibri", 11, XFontStyle.Regular);

            //Title Header
            gfx.DrawString("Persoonlijk Curriculum", Calibri20Bold, XBrushes.Black,
                           new XRect(0, 20, page.Width, 0), XStringFormats.Center);
            gfx.DrawString("ITvitae learning", Calibri20BoldItalic, XBrushes.Black,
                           new XRect(0, 41, page.Width, 0), XStringFormats.Center);
            //XImage ITvitaeLogo = XImage.FromFile(string.Empty);
            //gfx.DrawImage(ITvitaeLogo, new XRect(0, 0, 0, 0));

            //30-100-30 -100 -75-100-30 -100 -30
            //0 |30 |130|160|260|335|435|465|565|595
            //Student Info
            int col1x = 30; int col2x = 135; int col3x = 335; int col4x = 435;
            int colwidth = 100; int rowheight = 12;
            int row1 = 60; int row2 = 80; int row3 = 100;

            DrawLabel("Naam:", col1x, row1, colwidth, rowheight, gfx);
            DrawLabel(student.VolledigeNaam, col2x, row1, colwidth, rowheight, gfx);

            DrawLabel("Geboortedatum:", col1x, row2, colwidth, rowheight, gfx);
            DrawLabel(pdfvm.DoB.ToShortDateString(), col2x, row2, colwidth, rowheight, gfx);

            DrawLabel("StartDatum:", col1x, row3, colwidth, rowheight, gfx);
            DrawLabel(pdfvm.StartDate.ToShortDateString(), col2x, row3, colwidth, rowheight, gfx);

            DrawLabel("Einddatum:", col3x, row3, colwidth, rowheight, gfx);
            DrawLabel(pdfvm.EndDate.ToShortDateString(), col4x, row3, colwidth, rowheight, gfx);

            DrawLabel("Niveau:", col3x, row1, colwidth, rowheight, gfx);
            DrawLabel(pdfvm.Niveau, col4x, row1, colwidth, rowheight, gfx);

            DrawLabel("Richting:", col3x, row2, colwidth, rowheight, gfx);
            DrawLabel(pdfvm.Course, col4x, row2, colwidth, rowheight, gfx);

            //Topics
            //595 into 3 columns starting on Y = 150
            int YPointer = 150;
            int topicColumnWidth = 159;
            int topicColumn1 = 30; int topicColumn2 = 30 + topicColumnWidth; int topicColumn3 = 2 * topicColumnWidth + 60;
            XFont certFont = new XFont("calibri", 11, XFontStyle.Regular);
            XPen pen = new XPen(XColors.LightGray);

            //Add headers
            DrawLabel("Modules", topicColumn1, YPointer, topicColumnWidth, rowheight, gfx);
            if (pdfvm.Leerdoel)
            {
                DrawLabel("Leerdoelen", topicColumn2, YPointer, topicColumnWidth, rowheight, gfx);
            }
            if (pdfvm.Certificeringen)
            {
                DrawLabel("Certificeringen", topicColumn3, YPointer, topicColumnWidth, rowheight, gfx);
            }
            YPointer += rowheight;

            //List<Topic> topics = curriculum.Topics.ToList();
            List<CurriculumTopic> topics = curriculum.Topics.ToList();
            for (int i = 0; i < curriculum.Topics.Count; i++)
            {
                List<DrawStringListItem> drawList = new List<DrawStringListItem>();

                //Update YPointer;
                YPointer += 4;
                CurriculumTopic topic = topics[i];

                //measure topic
                XSize topicSize = gfx.MeasureString(topic.Topic.NaamCode, certFont);
                //make rect
                int rectWidth = topicColumnWidth;
                double rectRows = 1;
                int CertYPointer = 0;
                if (topicSize.Width > topicColumnWidth)
                {
                    rectRows = topicSize.Width / topicColumnWidth;
                    if (rectRows % 1 > 0)
                    {
                        rectRows++;
                    }
                    rectRows = Math.Floor(rectRows);
                }
                //check if new page is needed
                if (YPointer + (rowheight * rectRows) > 780)
                {
                    gfx = NewPage(document, out page, gfx, out tf, out YPointer, logo);
                }
                //Make topicrect
                XRect topicRect = new XRect(topicColumn1, 0, rectWidth, rowheight * rectRows);
                //Add string to drawlist
                drawList.Add(new DrawStringListItem(topic.Topic.NaamCode, certFont, XBrushes.Black, topicRect));

                if (pdfvm.Leerdoel)
                {
                    //measure leerdoel
                    XSize leerdoelSize = gfx.MeasureString(topic.Topic.Leerdoel, certFont);
                    //make rect
                    if (leerdoelSize.Width / topicColumnWidth > rectRows)
                    {
                        rectRows = leerdoelSize.Width / topicColumnWidth;
                        if (rectRows % 1 > 0)
                        {
                            rectRows++;
                        }
                        rectRows = Math.Floor(rectRows);
                    }
                    //check if new page is needed
                    if (YPointer + (rowheight * rectRows) > 780)
                    {
                        gfx = NewPage(document, out page, gfx, out tf, out YPointer, logo);
                    }
                    XRect leerdoelRect = new XRect(topicColumn2, 0, rectWidth, rowheight * rectRows);
                    //update height and y pointer
                    drawList.Add(new DrawStringListItem(topic.Topic.Leerdoel, certFont, XBrushes.Black, leerdoelRect));
                }

                if (pdfvm.Certificeringen)
                {
                    List<Certificering> certs = topic.Topic.Certificeringen.ToList();
                    for (int j = 0; j < certs.Count; j++)
                    {
                        //measure each cert
                        XSize certSize = gfx.MeasureString(certs[j].Naam, certFont);
                        //make rect for each cert
                        double certRows = 1;
                        if (certSize.Width / topicColumnWidth > certRows)
                        {
                            certRows = certSize.Width / topicColumnWidth;
                            if (certRows % 1 > 0)
                            {
                                certRows++;
                            }
                            certRows = Math.Floor(certRows);
                        }
                        XRect certRect = new XRect(topicColumn3, CertYPointer, rectWidth, rowheight * certRows);
                        //Add each cert to drawlist
                        drawList.Add(new DrawStringListItem(certs[j].Naam, certFont, XBrushes.Black, certRect));
                        //update CertYPointer for the next cert
                        CertYPointer += rowheight * (int)certRows;
                    }
                    //Check if certs go over the footer
                    if (YPointer + CertYPointer > 780)
                    {
                        gfx = NewPage(document, out page, gfx, out tf, out YPointer, logo);
                    }

                }

                //Draw all the things relative to YPointer
                gfx.DrawLine(pen, topicColumn1, YPointer - 2, topicColumn3 + topicColumnWidth, YPointer - 2);
                foreach (var item in drawList)
                {
                    XRect r = new XRect(item.Rect.X, item.Rect.Y + YPointer, item.Rect.Width, item.Rect.Height);
                    tf.DrawString(item.Text, item.Font, item.Brush, r);
                }
                YPointer += (rowheight * (int)rectRows) > CertYPointer ? (rowheight * (int)rectRows) : CertYPointer;
            }

            //add footers
            gfx.Dispose();
            int pageNumber = 1; int outOf = document.Pages.Count;
            foreach (PdfPage pdfPage in document.Pages)
            {
                gfx = XGraphics.FromPdfPage(pdfPage);
                int footerY = 802;
                XFont footerFont = new XFont("verdana", 10, XFontStyle.Italic);
                gfx.DrawLine(pen, 0, footerY, page.Width, footerY);
                gfx.DrawString($"Pagina {pageNumber}/{outOf}", footerFont, XBrushes.SlateGray, new XPoint(455, footerY + 20));
                gfx.DrawString("Versie 1.0", footerFont, XBrushes.SlateGray, new XPoint(65, footerY + 20));
                pageNumber++;
            }
            

            
            string filename = student + $"{ DateTime.Now.Millisecond.ToString()}.pdf";

            // $"Hello_{DateTime.Now.Millisecond.ToString()}.pdf";
            document.Save(filename);
            Process.Start(filename);
        }

        private static XGraphics NewPage(PdfDocument document, out PdfPage page, XGraphics gfx, out XTextFormatter tf, out int YPointer, XImage logo)
        {
            gfx.Dispose();
            page = document.AddPage();
            XGraphics g = XGraphics.FromPdfPage(page);
            tf = new XTextFormatter(g);
            YPointer = 40;

            //put logo on page before returning
            gfx.DrawImage(logo, new XPoint(595 - 191 - 30, 20));

            return g;
        }

        private static void DrawLabel(string text, int x, int y, int width, int height, XGraphics gfx)
        {
            XFont LabelFont = new XFont("calibri", 11, XFontStyle.Regular);
            gfx.DrawString(text, LabelFont, XBrushes.Black, new XRect(x, y, width, height), XStringFormats.CenterLeft);
        }
    }

    /// <summary>
    /// DrawStringListItems are used to stage drawstring operations to be drawn simultaneously
    /// after it is made sure the topic and contents do not exceed the page limit
    /// </summary>
    public class DrawStringListItem
    {
        public DrawStringListItem(string text, XFont font, XBrush brush, XRect rect)
        {
            this.Text = text;
            this.Font = font;
            this.Brush = brush;
            this.Rect = rect;
        }

        public string Text { get; set; }
        public XFont Font { get; set; }
        public XBrush Brush { get; set; }
        public XRect Rect { get; set; }
    }
}

