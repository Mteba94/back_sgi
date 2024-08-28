using iTextSharp.text;
using iTextSharp.text.pdf;
using WebApi_SGI_T.Models.Certification;

namespace WebApi_SGI_T.Imp
{
    public class CertificationService
    {
        private readonly IConfiguration _configuracion;
        public CertificationService(IConfiguration configuration)
        {
            _configuracion = configuration;
        }
        /*public string GeneratedPdfBase64(CertificationModel model)
        {
           
            string templatePath = @"Models\Certification\html\bautismos.html";
            string htmlTemplate = System.IO.File.ReadAllText(templatePath);

            // Reemplazar los placeholders en el template HTML
            string htmlContent = htmlTemplate
                .Replace("{numero}", model.Numero)
                .Replace("{folio}", model.Folio)
                .Replace("{partida}", model.Partida)
                .Replace("{dia}", model.Dia)
                .Replace("{mes}", model.Mes)
                .Replace("{anio}", model.Anio)
                .Replace("{nombreBautizado}", model.NombreBautizado)
                .Replace("{fechaNacimiento}", model.FechaNacimiento)
                .Replace("{nombrePadre}", model.NombrePadre)
                .Replace("{nombreMadre}", model.NombreMadre)
                .Replace("{nombrePadrinos}", model.NombrePadrinos)
                .Replace("{nombreSacerdote}", model.NombreSacerdote)
                .Replace("{anotacionMarginal}", model.AnotacionMarginal)
                .Replace("{diaExpedicion}", model.DiaExpedicion)
                .Replace("{mesExpedicion}", model.MesExpedicion)
                .Replace("{anioExpedicion}", model.AnioExpedicion);

            // Crear una instancia del convertidor HTML a PDF
            var converter = new HtmlToPdf();

            // Configurar el tamaño de la página a tamaño carta
            converter.Options.PdfPageSize = PdfPageSize.Letter; // Tamaño carta (8.5 x 11 pulgadas)
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait; // Orientación de la página

            // Convertir el HTML a PDF
            PdfDocument pdf = converter.ConvertHtmlString(htmlContent);

            // Convertir el PDF a una cadena Base64
            byte[] pdfBytes = pdf.Save();
            return Convert.ToBase64String(pdfBytes);
        }*/

        public string GeneratedPdfBase64(CertificationModel model)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Crear el documento PDF
                Document doc = new Document(PageSize.LETTER);
                doc.SetMargins(28.34f, 28.34f, 28.34f, 28.34f);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);

                // Abrir el documento para agregar contenido
                doc.Open();

                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                string fontPath = @"Static\Fonts\Montserrat.ttf";

                BaseFont font = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                iTextSharp.text.Font titulo = new iTextSharp.text.Font(font, 14f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font subtitulo = new iTextSharp.text.Font(font, 11f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font normal = new iTextSharp.text.Font(font, 9f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(@"Models\Certification\img\logo-parroquia.png");
                logo.ScaleAbsoluteHeight(70);
                logo.ScaleAbsoluteWidth(70);
                logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;

                doc.Add(logo);
                doc.Add(new Paragraph(_configuracion["Certification:Church"], titulo)
                {
                    Alignment = Element.ALIGN_CENTER,
                    Leading = 10f
                });
                doc.Add(new Paragraph(_configuracion["Certification:Parish"], subtitulo)
                {
                    Alignment = Element.ALIGN_CENTER,
                    Leading = 12f
                });
                doc.Add(new Paragraph(_configuracion["Certification:Address"], normal)
                {
                    Alignment = Element.ALIGN_CENTER,
                    Leading = 12f
                });
                doc.Add(new Paragraph(_configuracion["Certification:Contact"], normal)
                {
                    Alignment = Element.ALIGN_CENTER,
                    Leading = 12f
                });
                doc.Add(new Paragraph(" ")
                {
                    Leading = 12f,
                    SpacingAfter = 10
                });
                doc.Add(new Paragraph($"Constancia de {model.TipoSacramento}", titulo)
                {
                    Alignment = Element.ALIGN_CENTER
                });
                doc.Add(new Paragraph(" ")
                {
                    SpacingAfter = 10
                });
                doc.Add(new Paragraph(_configuracion["Certification:ViewText"])
                {
                    Alignment = Element.ALIGN_CENTER
                });

                doc.Add(new Paragraph(" ") { SpacingAfter = 3 });

                PdfPTable numPartidas = new PdfPTable(3);
                numPartidas.WidthPercentage = 100;
                numPartidas.SetWidths(new float[] { 33, 33, 33 });

                numPartidas.AddCell(new PdfPCell(new Phrase($"No: {model.Numero}", normal))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
                numPartidas.AddCell(new PdfPCell(new Phrase($"Folio: {model.Folio}", normal))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
                numPartidas.AddCell(new PdfPCell(new Phrase($"Partida No: {model.Partida}", normal))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER
                });

                doc.Add(numPartidas);
                doc.Add(new Paragraph(" ") { SpacingAfter = 3 });

                doc.Add(new Paragraph($"En la Parroquia Inmaculada Concepcion de María el {model.Dia} de {model.Mes} del año {model.Anio}"));

                doc.Add(new Paragraph(" ") { SpacingAfter = 1 });

                PdfPTable nombre = new PdfPTable(2);
                nombre.WidthPercentage = 100;
                nombre.SetWidths(new float[] {30, 70});

                nombre.AddCell(new PdfPCell(new Phrase("Se Bautizó a:", normal))
                {
                    Border = Rectangle.NO_BORDER
                });

                nombre.AddCell(new PdfPCell(new Phrase(model.NombreBautizado, normal))
                {
                    Border = Rectangle.NO_BORDER
                });

                doc.Add(nombre);

                PdfPTable nacimiento = new PdfPTable(2);
                nacimiento.WidthPercentage = 100;
                nacimiento.SetWidths(new float[] {30, 70});

                nacimiento.AddCell(new PdfPCell(new Phrase("Que nació:", normal))
                {
                    Border = Rectangle.NO_BORDER
                });

                nacimiento.AddCell(new PdfPCell(new Phrase(model.FechaNacimiento, normal))
                {
                    Border = Rectangle.NO_BORDER
                });

                doc.Add(nacimiento);

                PdfPTable padre = new PdfPTable(2);
                padre.WidthPercentage = 100;
                padre.SetWidths(new float[] {30, 70 });

                padre.AddCell(new PdfPCell(new Phrase("Hijo(a) de:", normal))
                {
                    Border = Rectangle.NO_BORDER
                });

                padre.AddCell(new PdfPCell(new Phrase(model.NombrePadre, normal))
                {
                    Border = Rectangle.NO_BORDER
                });

                doc.Add(padre);

                PdfPTable madre = new PdfPTable(2);
                madre.WidthPercentage = 100;
                madre.SetWidths(new float[] {30, 70 });

                madre.AddCell(new PdfPCell(new Phrase("Y de:", normal))
                {
                    Border = Rectangle.NO_BORDER
                });

                madre.AddCell(new PdfPCell(new Phrase(model.NombreMadre, normal))
                {
                    Border = Rectangle.NO_BORDER
                });

                doc.Add(madre);

                PdfPTable padrinos = new PdfPTable(2);
                padrinos.WidthPercentage = 100;
                padrinos.SetWidths(new float[] {30, 70 });

                padrinos.AddCell(new PdfPCell(new Phrase("Padrinos:", normal))
                {
                    Border = Rectangle.NO_BORDER
                });

                padrinos.AddCell(new PdfPCell(new Phrase(model.NombrePadrinos, normal))
                {
                    Border = Rectangle.NO_BORDER
                });

                doc.Add(padrinos);

                PdfPTable sacerdote = new PdfPTable(2);
                sacerdote.WidthPercentage = 100;
                sacerdote.SetWidths(new float[] {30, 70 });

                sacerdote.AddCell(new PdfPCell(new Phrase("Realizo el Bautismo el Pbro:", normal))
                {
                    Border = Rectangle.NO_BORDER
                });

                sacerdote.AddCell(new PdfPCell(new Phrase(model.NombreSacerdote, normal))
                {
                    Border = Rectangle.NO_BORDER
                });

                doc.Add(sacerdote);

                PdfPTable anotacion = new PdfPTable(2);
                anotacion.WidthPercentage = 100;
                anotacion.SetWidths(new float[] {30, 70 });

                anotacion.AddCell(new PdfPCell(new Phrase("En la anotacion marginal se lee:", normal))
                {
                    Border = Rectangle.NO_BORDER
                });

                anotacion.AddCell(new PdfPCell(new Phrase(model.AnotacionMarginal, normal))
                {
                    Border = Rectangle.NO_BORDER
                });

                doc.Add(anotacion);

                doc.Add(new Paragraph(" ") { SpacingAfter = 3 });

                PdfPTable realizado = new PdfPTable(1);
                realizado.WidthPercentage = 100;

                realizado.AddCell(new PdfPCell(new Phrase($"Se extiende, firma y sella la presente en la Oficina Parroquial en Guatemala de la Asunción, {model.Dia} de {model.Mes} del año {model.Anio}", normal))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER
                });

                doc.Add(realizado);

                doc.Add(new Paragraph(" ") { SpacingAfter = 30 });

                doc.Add(new Paragraph($"Pbro. {model.NombreSacerdote}, FMM", subtitulo)
                {
                    SpacingBefore = 100,
                    Alignment = Element.ALIGN_CENTER
                });
                
                doc.Add(new Paragraph("Vicario Parroquial", normal)
                {
                    Alignment = Element.ALIGN_CENTER
                });



                doc.Close();

                // Convertir el PDF a una cadena Base64
                byte[] pdfBytes = ms.ToArray();
                return Convert.ToBase64String(pdfBytes);
            }
        }
    }
}
