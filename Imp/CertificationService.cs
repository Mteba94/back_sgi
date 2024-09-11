using iTextSharp.text;
using iTextSharp.text.pdf;
using WebApi_SGI_T.Models.Certification;
using WebApi_SGI_T.Models.Commons.Request;
using WebApi_SGI_T.Models.Commons.Response;
using WebApi_SGI_T.Static;

namespace WebApi_SGI_T.Imp
{
    public class CertificationService
    {
        private readonly IConfiguration _configuracion;
        private readonly HistoricoConstanciasService _historicoConstanciasService;
        public CertificationService(IConfiguration configuration, HistoricoConstanciasService historicoConstanciasService)
        {
            _configuracion = configuration;
            _historicoConstanciasService = historicoConstanciasService;
        }
       
        public BaseResponse<ConstanciaResponse> GeneratedPdfBase64(CertificationModel model)
        {
            var response = new BaseResponse<ConstanciaResponse>();

            using (MemoryStream ms = new MemoryStream())
            {

                Document doc = new Document(PageSize.LETTER);
                doc.SetMargins(28.34f, 28.34f, 28.34f, 28.34f);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);

                doc.Open();

                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                string fontPath = @"Static\Fonts\Montserrat.ttf";

                BaseFont font = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                iTextSharp.text.Font titulo = new iTextSharp.text.Font(font, 14f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font subtitulo = new iTextSharp.text.Font(font, 11f, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font Tnormal = new iTextSharp.text.Font(font, 9f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font normal = new iTextSharp.text.Font(font, 10f, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0));

                AgregarEncabezado(doc, model, titulo, subtitulo, Tnormal);

                switch (model.idTipoSacramento)
                {
                    case 1:
                        GenerarConstanciaBautismo(doc, model, titulo, subtitulo, normal);
                        break;
                    case 2:
                        GenerarConstanciaPrimeraComunion(doc, model, titulo, subtitulo, normal);
                        break;
                    case 3:
                        GenerarConstanciaConfirmacion(doc, model, titulo, subtitulo, normal);
                        break;
                    case 4:
                        GenerarConstanciaMatrimonio(doc, model, titulo, subtitulo, normal);
                        break;
                    default:
                        throw new ArgumentException("Tipo de sacramento no soportado.");
                }

                AgregarCorrelativo(doc, writer, model, titulo, subtitulo, Tnormal);

                // Cerrar el documento PDF
                doc.Close();

                // Convertir el PDF a una cadena Base64
                byte[] pdfBytes = ms.ToArray();
                var B64 = Convert.ToBase64String(pdfBytes);

                var mappedData = new ConstanciaResponse
                {
                    FileName = $"{model.TipoSacramento}.pdf",
                    B64 = B64
                };

                if (pdfBytes.Length > 0)
                {
                    response.IsSuccess = true;
                    response.Data = mappedData;
                    response.Message = ReplyMessage.MESSAGE_QUERY;

                }
                else
                {
                    response.IsSuccess = false;
                    response.Message += ReplyMessage.MESSAGE_QUERY_EMPTY;
                }
            }

            return response;
        }

        private void AgregarEncabezado(Document doc, CertificationModel model, iTextSharp.text.Font titulo, iTextSharp.text.Font subtitulo, iTextSharp.text.Font Tnormal)
        {
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

            doc.Add(new Paragraph(_configuracion["Certification:Address"], Tnormal)
            {
                Alignment = Element.ALIGN_CENTER,
                Leading = 12f
            });

            doc.Add(new Paragraph(_configuracion["Certification:Contact"], Tnormal)
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

        }

        private void GenerarConstanciaBautismo(Document doc, CertificationModel model, iTextSharp.text.Font titulo, iTextSharp.text.Font subtitulo, iTextSharp.text.Font normal)
        {

            string certificacion = "Hace constar que en Registro de esta Parroquia según el Libro de Bautismo";

            doc.Add(new Paragraph(certificacion, subtitulo)
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

            doc.Add(new Paragraph($"En la Parroquia Inmaculada Concepcion de María el {model.Dia} de {model.Mes} del año {model.Anio}", subtitulo));

            doc.Add(new Paragraph(" ") { SpacingAfter = 1 });

            string nombrePersona = "Se Bautizó a:";

            PdfPTable nombre = new PdfPTable(2);
            nombre.WidthPercentage = 100;
            nombre.SetWidths(new float[] { 30, 70 });

            nombre.AddCell(new PdfPCell(new Phrase(nombrePersona, normal))
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
            nacimiento.SetWidths(new float[] { 30, 70 });

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
            padre.SetWidths(new float[] { 30, 70 });

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
            madre.SetWidths(new float[] { 30, 70 });

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
            padrinos.SetWidths(new float[] { 30, 70 });

            padrinos.AddCell(new PdfPCell(new Phrase("Padrinos:", normal))
            {
                Border = Rectangle.NO_BORDER
            });
            padrinos.AddCell(new PdfPCell(new Phrase(model.NombrePadrinos?[0], normal))
            {
                Border = Rectangle.NO_BORDER
            });

            doc.Add(padrinos);

            PdfPTable madrina = new PdfPTable(2);
            madrina.WidthPercentage = 100;
            madrina.SetWidths(new float[] { 30, 70 });

            madrina.AddCell(new PdfPCell(new Phrase("", normal))
            {
                Border = Rectangle.NO_BORDER
            });
            madrina.AddCell(new PdfPCell(new Phrase(model.NombrePadrinos?[1], normal))
            {
                Border = Rectangle.NO_BORDER
            });

            doc.Add(madrina);

            PdfPTable sacerdote = new PdfPTable(2);
            sacerdote.WidthPercentage = 100;
            sacerdote.SetWidths(new float[] { 30, 70 });

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
            anotacion.SetWidths(new float[] { 30, 70 });

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

            realizado.AddCell(new PdfPCell(new Phrase($"Se extiende, firma y sella la presente en la Oficina Parroquial en Guatemala de la Asunción, {model.DiaExpedicion} de {model.MesExpedicion} del año {model.AnioExpedicion}", normal))
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
        }

        private void GenerarConstanciaPrimeraComunion(Document doc, CertificationModel model, iTextSharp.text.Font titulo, iTextSharp.text.Font subtitulo, iTextSharp.text.Font normal)
        {
            string? certi_configuracion = _configuracion["Certification:ViewText"];

            string certificacion = $"{certi_configuracion} {model.TipoSacramento}";

            doc.Add(new Paragraph(certificacion, subtitulo)
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

            string nombrePersona = "Se encuentra inscrito (a):";

            PdfPTable nombre = new PdfPTable(2);
            nombre.WidthPercentage = 100;
            nombre.SetWidths(new float[] { 30, 70 });

            nombre.AddCell(new PdfPCell(new Phrase(nombrePersona, normal))
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
            nacimiento.SetWidths(new float[] { 30, 70 });

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
            padre.SetWidths(new float[] { 30, 70 });

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
            madre.SetWidths(new float[] { 30, 70 });

            madre.AddCell(new PdfPCell(new Phrase("Y de:", normal))
            {
                Border = Rectangle.NO_BORDER
            });

            madre.AddCell(new PdfPCell(new Phrase(model.NombreMadre, normal))
            {
                Border = Rectangle.NO_BORDER
            });

            doc.Add(madre);

            PdfPTable FechaSacramento = new PdfPTable(2);
            FechaSacramento.WidthPercentage = 100;
            FechaSacramento.SetWidths(new float[] { 30, 70 });

            FechaSacramento.AddCell(new PdfPCell(new Phrase($"Fecha de {model.TipoSacramento}:", normal))
            {
                Border = Rectangle.NO_BORDER
            });
            FechaSacramento.AddCell(new PdfPCell(new Phrase($"{model.Dia} de {model.Mes} del año {model.Anio}", normal))
            {
                Border = Rectangle.NO_BORDER
            });

            doc.Add(FechaSacramento);

            PdfPTable anotacion = new PdfPTable(2);
            anotacion.WidthPercentage = 100;
            anotacion.SetWidths(new float[] { 30, 70 });

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

            realizado.AddCell(new PdfPCell(new Phrase($"Se extiende, firma y sella la presente en la Oficina Parroquial en Guatemala de la Asunción, {model.DiaExpedicion} de {model.MesExpedicion} del año {model.AnioExpedicion}", normal))
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
        }

        private void GenerarConstanciaConfirmacion(Document doc, CertificationModel model, iTextSharp.text.Font titulo, iTextSharp.text.Font subtitulo, iTextSharp.text.Font normal)
        {

            string? certi_configuracion = _configuracion["Certification:ViewText"];

            string certificacion = $"{certi_configuracion} {model.TipoSacramento}";

            doc.Add(new Paragraph(certificacion, subtitulo)
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

            string nombrePersona = "Se encuentra inscrito (a):";

            PdfPTable nombre = new PdfPTable(2);
            nombre.WidthPercentage = 100;
            nombre.SetWidths(new float[] { 30, 70 });

            nombre.AddCell(new PdfPCell(new Phrase(nombrePersona, normal))
            {
                Border = Rectangle.NO_BORDER
            });

            nombre.AddCell(new PdfPCell(new Phrase(model.NombreBautizado, normal))
            {
                Border = Rectangle.NO_BORDER
            });

            doc.Add(nombre);

            PdfPTable FechaSacramento = new PdfPTable(2);
            FechaSacramento.WidthPercentage = 100;
            FechaSacramento.SetWidths(new float[] { 30, 70 });

            FechaSacramento.AddCell(new PdfPCell(new Phrase($"Fecha de {model.TipoSacramento}:", normal))
            {
                Border = Rectangle.NO_BORDER
            });
            FechaSacramento.AddCell(new PdfPCell(new Phrase($"{model.Dia} de {model.Mes} del año {model.Anio}", normal))
            {
                Border = Rectangle.NO_BORDER
            });

            doc.Add(FechaSacramento);

            PdfPTable padre = new PdfPTable(2);
            padre.WidthPercentage = 100;
            padre.SetWidths(new float[] { 30, 70 });

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
            madre.SetWidths(new float[] { 30, 70 });

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
            padrinos.SetWidths(new float[] { 30, 70 });

            padrinos.AddCell(new PdfPCell(new Phrase("Padrinos:", normal))
            {
                Border = Rectangle.NO_BORDER
            });
            padrinos.AddCell(new PdfPCell(new Phrase(model.NombrePadrinos?[0], normal))
            {
                Border = Rectangle.NO_BORDER
            });

            doc.Add(padrinos);

            PdfPTable madrina = new PdfPTable(2);
            madrina.WidthPercentage = 100;
            madrina.SetWidths(new float[] { 30, 70 });

            madrina.AddCell(new PdfPCell(new Phrase("", normal))
            {
                Border = Rectangle.NO_BORDER
            });
            madrina.AddCell(new PdfPCell(new Phrase(model.NombrePadrinos?[1], normal))
            {
                Border = Rectangle.NO_BORDER
            });

            doc.Add(madrina);

            PdfPTable sacerdote = new PdfPTable(1);
            sacerdote.WidthPercentage = 100;
            sacerdote.SetWidths(new float[] { 100 });

            sacerdote.AddCell(new PdfPCell(new Phrase($"Quien fue confirmado(a) a los {model.edad} años de edad, por el Señor Obispo Mons. {model.NombreSacerdote}", normal))
            {
                Border = Rectangle.NO_BORDER
            });

            doc.Add(sacerdote);

            PdfPTable anotacion = new PdfPTable(2);
            anotacion.WidthPercentage = 100;
            anotacion.SetWidths(new float[] { 30, 70 });

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

            realizado.AddCell(new PdfPCell(new Phrase($"Se extiende, firma y sella la presente en la Oficina Parroquial en Guatemala de la Asunción, {model.DiaExpedicion} de {model.MesExpedicion} del año {model.AnioExpedicion}", normal))
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
        }

        private void GenerarConstanciaMatrimonio(Document doc, CertificationModel model, iTextSharp.text.Font titulo, iTextSharp.text.Font subtitulo, iTextSharp.text.Font normal)
        {
            string? certi_configuracion = _configuracion["Certification:ViewText"];

            string certificacion = $"{certi_configuracion} {model.TipoSacramento}";

            doc.Add(new Paragraph(certificacion, subtitulo)
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

            string nombrePersona = "Se encuentra inscrito:";

            PdfPTable nombre = new PdfPTable(2);
            nombre.WidthPercentage = 100;
            nombre.SetWidths(new float[] { 30, 70 });

            nombre.AddCell(new PdfPCell(new Phrase(nombrePersona, normal))
            {
                Border = Rectangle.NO_BORDER
            });

            nombre.AddCell(new PdfPCell(new Phrase(model.NombreBautizado, normal))
            {
                Border = Rectangle.NO_BORDER
            });

            doc.Add(nombre);

            PdfPTable padre = new PdfPTable(2);
            padre.WidthPercentage = 100;
            padre.SetWidths(new float[] { 30, 70 });

            padre.AddCell(new PdfPCell(new Phrase("Hijo de:", normal))
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
            madre.SetWidths(new float[] { 30, 70 });

            madre.AddCell(new PdfPCell(new Phrase("Y de:", normal))
            {
                Border = Rectangle.NO_BORDER
            });

            madre.AddCell(new PdfPCell(new Phrase(model.NombreMadre, normal))
            {
                Border = Rectangle.NO_BORDER
            });

            doc.Add(madre);

            string nombrePersona2 = "Quien contrajo Matrimonio con:";

            PdfPTable nombre2 = new PdfPTable(2);
            nombre2.WidthPercentage = 100;
            nombre2.SetWidths(new float[] { 30, 70 });

            nombre2.AddCell(new PdfPCell(new Phrase(nombrePersona2, normal))
            {
                Border = Rectangle.NO_BORDER
            });

            nombre2.AddCell(new PdfPCell(new Phrase(model.NombreEsposa, normal))
            {
                Border = Rectangle.NO_BORDER
            });

            doc.Add(nombre2);

            PdfPTable padre2 = new PdfPTable(2);
            padre2.WidthPercentage = 100;
            padre2.SetWidths(new float[] { 30, 70 });

            padre2.AddCell(new PdfPCell(new Phrase("Hija de:", normal))
            {
                Border = Rectangle.NO_BORDER
            });

            padre2.AddCell(new PdfPCell(new Phrase(model.NombrePadreEsposa, normal))
            {
                Border = Rectangle.NO_BORDER
            });

            doc.Add(padre2);

            PdfPTable madre2 = new PdfPTable(2);
            madre2.WidthPercentage = 100;
            madre2.SetWidths(new float[] { 30, 70 });

            madre2.AddCell(new PdfPCell(new Phrase("Y de:", normal))
            {
                Border = Rectangle.NO_BORDER
            });

            madre2.AddCell(new PdfPCell(new Phrase(model.NombreMadreEsposa, normal))
            {
                Border = Rectangle.NO_BORDER
            });

            doc.Add(madre2);

            PdfPTable fechaSacramento = new PdfPTable(1);
            fechaSacramento.WidthPercentage = 100;
            fechaSacramento.SetWidths(new float[] { 100 });

            fechaSacramento.AddCell(new PdfPCell(new Phrase($"El día {model.Dia} de {model.Mes} del año {model.Anio}", normal))
            {
                Border = Rectangle.NO_BORDER
            });

            doc.Add(fechaSacramento);

            PdfPTable padrinos = new PdfPTable(2);
            padrinos.WidthPercentage = 100;
            padrinos.SetWidths(new float[] { 30, 70 });

            padrinos.AddCell(new PdfPCell(new Phrase("Padrinos:", normal))
            {
                Border = Rectangle.NO_BORDER
            });
            padrinos.AddCell(new PdfPCell(new Phrase(model.NombrePadrinos?[0], normal))
            {
                Border = Rectangle.NO_BORDER
            });

            doc.Add(padrinos);

            PdfPTable madrina = new PdfPTable(2);
            madrina.WidthPercentage = 100;
            madrina.SetWidths(new float[] { 30, 70 });

            madrina.AddCell(new PdfPCell(new Phrase("", normal))
            {
                Border = Rectangle.NO_BORDER
            });
            madrina.AddCell(new PdfPCell(new Phrase(model.NombrePadrinos?[1], normal))
            {
                Border = Rectangle.NO_BORDER
            });

            doc.Add(madrina);

            PdfPTable sacerdote = new PdfPTable(2);
            sacerdote.WidthPercentage = 100;
            sacerdote.SetWidths(new float[] { 30, 70 });

            sacerdote.AddCell(new PdfPCell(new Phrase("Celebro dicho Matrimonio:", normal))
            {
                Border = Rectangle.NO_BORDER
            });

            sacerdote.AddCell(new PdfPCell(new Phrase(model.NombreSacerdote, normal))
            {
                Border = Rectangle.NO_BORDER
            });

            doc.Add(sacerdote);

            PdfPTable realizado = new PdfPTable(1);
            realizado.WidthPercentage = 100;

            realizado.AddCell(new PdfPCell(new Phrase($"Se extiende, firma y sella la presente en la Oficina Parroquial en Guatemala de la Asunción, {model.DiaExpedicion} de {model.MesExpedicion} del año {model.AnioExpedicion}", normal))
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
        }

        private void AgregarCorrelativo(Document doc, PdfWriter writer, CertificationModel model, iTextSharp.text.Font titulo, iTextSharp.text.Font subtitulo, iTextSharp.text.Font Tnormal)
        {
            string correlativo = model.correlativo;

            PdfContentByte cb = writer.DirectContent;

            // Definir la fuente y tamaño
            BaseFont baseFont = Tnormal.BaseFont;
            cb.SetFontAndSize(baseFont, Tnormal.Size);

            // Posiciones
            float pageWidth = writer.PageSize.Width;
            float pageHeight = writer.PageSize.Height;
            float marginBottom = doc.BottomMargin + 10f; // Ajustar según el diseño
            float marginRight = doc.RightMargin;

            // Añadir el correlativo al pie de página, alineado a la derecha
            cb.BeginText();
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, correlativo, pageWidth - marginRight, marginBottom, 0);
            cb.EndText();
        }
    }
}
