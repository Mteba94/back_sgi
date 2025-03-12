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
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuracion;
        private readonly HistoricoConstanciasService _historicoConstanciasService;
        public CertificationService(IConfiguration configuration, HistoricoConstanciasService historicoConstanciasService, IWebHostEnvironment env)
        {
            _configuracion = configuration;
            _historicoConstanciasService = historicoConstanciasService;
            _env = env;
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

                var webRootPaht = _env.WebRootPath;

                string fontPath = $"{webRootPaht}/certificacion/Montserrat.ttf";
                string fontPathRegular = $"{webRootPaht}/certificacion/Montserrat-Regular.ttf";
                string fontPathBold = $"{webRootPaht}/certificacion/Montserrat-Bold.ttf";
                string imagePath = $"{webRootPaht}/certificacion/Logo Parroquial-03.png";

                BaseFont font = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                BaseFont fontR = BaseFont.CreateFont(fontPathRegular, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                BaseFont fontB = BaseFont.CreateFont(fontPathBold, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                iTextSharp.text.Font titulo = new iTextSharp.text.Font(fontB, 16f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font subtitulo = new iTextSharp.text.Font(fontB, 14f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font Tnormal = new iTextSharp.text.Font(font, 9f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font normal = new iTextSharp.text.Font(fontR, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font normalBold = new iTextSharp.text.Font(fontB, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                AgregarImagenFondo(writer, imagePath);

                AgregarEncabezado(doc, model, titulo, subtitulo, Tnormal);

                switch (model.idTipoSacramento)
                {
                    case 1:
                        GenerarConstanciaBautismo(doc, model, titulo, subtitulo, normal, normalBold);
                        break;
                    case 2:
                        GenerarConstanciaPrimeraComunion(doc, model, titulo, subtitulo, normal, normalBold);
                        break;
                    case 3:
                        GenerarConstanciaConfirmacion(doc, model, titulo, subtitulo, normal, normalBold);
                        break;
                    case 4:
                        GenerarConstanciaMatrimonio(doc, model, titulo, subtitulo, normal, normalBold);
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

                string fileName = "";

                if(model.idTipoSacramento == 4)
                {
                    fileName = $"{model.TipoSacramento} - {model.NombreBautizado} & {model.NombreEsposa}.pdf";
                }
                else
                {
                    fileName = $"{model.TipoSacramento} - {model.NombreBautizado}.pdf";
                }

                var mappedData = new ConstanciaResponse
                {
                    FileName = fileName,
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
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance($"{_env.WebRootPath}/certificacion/logo-parroquia.png");

            logo.ScaleAbsoluteHeight(70);
            logo.ScaleAbsoluteWidth(70);
            logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;

            doc.Add(logo);

            doc.Add(new Paragraph(_configuracion["Certification:Church"], titulo)
            {
                Alignment = Element.ALIGN_CENTER,
                Leading = 15f
            });

            doc.Add(new Paragraph(_configuracion["Certification:Parish"], subtitulo)
            {
                Alignment = Element.ALIGN_CENTER,
                Leading = 15f
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

        public void AgregarImagenFondo(PdfWriter writer, string imagePath)
        {
            PdfContentByte canvas = writer.DirectContentUnder;
            Image image = Image.GetInstance(imagePath);

            // Establecer la opacidad
            PdfGState state = new PdfGState();
            state.FillOpacity = 0.05f;
            canvas.SetGState(state);

            Rectangle pageSize = writer.PageSize;
            float pageWidth = pageSize.Width;
            float pageHeight = pageSize.Height;
            float imageWidth = PageSize.LETTER.Width;
            float imageHeight = PageSize.LETTER.Width;

            float xPosition = (pageWidth - imageWidth) / 2;
            float yPosition = (pageHeight - imageHeight) / 2;
            image.SetAbsolutePosition(xPosition, yPosition);
            image.ScaleAbsolute(imageWidth, imageHeight);

            //image.SetAbsolutePosition(0, 0);
            //image.ScaleAbsolute(PageSize.LETTER.Width, PageSize.LETTER.Width);

            // Agregar la imagen al documento
            canvas.AddImage(image);
        }

        private void GenerarConstanciaBautismo(Document doc, CertificationModel model, iTextSharp.text.Font titulo, iTextSharp.text.Font subtitulo, iTextSharp.text.Font normal, iTextSharp.text.Font normalBold)
        {

            float padLeft = 15f;

            string certificacion = "Hace constar que en Registro de esta Parroquia según el Libro de Bautismo";

            doc.Add(new Paragraph(certificacion, subtitulo)
            {
                Alignment = Element.ALIGN_CENTER
            });

            doc.Add(new Paragraph(" ") { SpacingAfter = 3 });

            PdfPTable numPartidas = new PdfPTable(3);
            numPartidas.WidthPercentage = 100;
            numPartidas.SetWidths(new float[] { 33, 33, 33 });

            Phrase phraseNumero = new Phrase();
            phraseNumero.Add(new Chunk("No: ", normal));
            phraseNumero.Add(new Chunk(model.Numero!.ToString(), normalBold));

            Phrase phraseFolio = new Phrase();
            phraseFolio.Add(new Chunk("Folio: ", normal));
            phraseFolio.Add(new Chunk(model.Folio, normalBold));

            Phrase phrasePartida = new Phrase();
            phrasePartida.Add(new Chunk("Partida No: ", normal));
            phrasePartida.Add(new Chunk(model.Partida, normalBold));

            numPartidas.AddCell(new PdfPCell(phraseNumero)
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            numPartidas.AddCell(new PdfPCell(phraseFolio)
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            numPartidas.AddCell(new PdfPCell(phrasePartida)
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            doc.Add(numPartidas);

            doc.Add(new Paragraph(" ") { SpacingAfter = 3 });

            //doc.Add(new Paragraph($"En la Parroquia Inmaculada Concepcion de María el {model.Dia} de {model.Mes} del año {model.Anio}", subtitulo));

            //doc.Add(new Paragraph(" ") { SpacingAfter = 1 });

            PdfPTable fechaBau = new PdfPTable(1);
            fechaBau.WidthPercentage = 100;
            fechaBau.SetWidths(new float[] { 100 });

            //fechaBau.AddCell(new PdfPCell(new Phrase(" ", normalBold))
            //{
            //    Border = Rectangle.NO_BORDER,
            //    HorizontalAlignment = Element.ALIGN_RIGHT
            //});

            fechaBau.AddCell(new PdfPCell(new Phrase($"El {model.Dia} de {model.Mes} del año {model.Anio}", normalBold))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            doc.Add(fechaBau);
            PdfPCell emptyCell = new PdfPCell(new Phrase(" ", normalBold))
            {
                Border = Rectangle.NO_BORDER,
                FixedHeight = 5f
            };

            PdfPTable emptyTable = new PdfPTable(1);
            emptyTable.AddCell(emptyCell);
            doc.Add(emptyTable);

            string nombrePersona = "Se Bautizó a:";

            PdfPTable nombre = new PdfPTable(2);
            nombre.WidthPercentage = 100;
            nombre.SetWidths(new float[] { 35, 65 });

            nombre.AddCell(new PdfPCell(new Phrase(nombrePersona, normalBold))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            nombre.AddCell(new PdfPCell(new Phrase(model.NombreBautizado, normalBold))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(nombre);

             
            doc.Add(emptyTable);

            PdfPTable nacimiento = new PdfPTable(2);
            nacimiento.WidthPercentage = 100;
            nacimiento.SetWidths(new float[] { 35, 65 });

            nacimiento.AddCell(new PdfPCell(new Phrase("Que nació:", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            nacimiento.AddCell(new PdfPCell(new Phrase(model.FechaNacimiento, normal))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(nacimiento);

            doc.Add(emptyTable);

            PdfPTable padre = new PdfPTable(2);
            padre.WidthPercentage = 100;
            padre.SetWidths(new float[] {35, 65 });

            padre.AddCell(new PdfPCell(new Phrase("Hijo(a) de:", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            padre.AddCell(new PdfPCell(new Phrase(model.NombrePadre, normal))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(padre);
            doc.Add(emptyTable);

            PdfPTable madre = new PdfPTable(2);
            madre.WidthPercentage = 100;
            madre.SetWidths(new float[] { 35, 65 });

            madre.AddCell(new PdfPCell(new Phrase("Y de:", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            madre.AddCell(new PdfPCell(new Phrase(model.NombreMadre, normal))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(madre);
            doc.Add(emptyTable);

            PdfPTable padrinos = new PdfPTable(2);
            padrinos.WidthPercentage = 100;
            padrinos.SetWidths(new float[] {35, 65 });

            padrinos.AddCell(new PdfPCell(new Phrase("Padrinos:", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });
            padrinos.AddCell(new PdfPCell(new Phrase(model.NombrePadrinos?[0], normal))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(padrinos);
            doc.Add(emptyTable);

            PdfPTable madrina = new PdfPTable(2);
            madrina.WidthPercentage = 100;
            madrina.SetWidths(new float[] {35, 65 });

            madrina.AddCell(new PdfPCell(new Phrase("", normal))
            {
                Border = Rectangle.NO_BORDER
            });
            madrina.AddCell(new PdfPCell(new Phrase(model.NombrePadrinos?[1], normal))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(madrina);
            doc.Add(emptyTable);

            PdfPTable sacerdote = new PdfPTable(2);
            sacerdote.WidthPercentage = 100;
            sacerdote.SetWidths(new float[] {35, 65 });

            sacerdote.AddCell(new PdfPCell(new Phrase($"Realizo el bautizo el {model.SacerdoteRealizaCat}", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            sacerdote.AddCell(new PdfPCell(new Phrase(model.NombreSacerdote, normal))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(sacerdote);
            doc.Add(emptyTable);

            PdfPTable anotacion = new PdfPTable(2);
            anotacion.WidthPercentage = 100;
            anotacion.SetWidths(new float[] {35, 65 });

            anotacion.AddCell(new PdfPCell(new Phrase("En la anotacion marginal se lee:", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            anotacion.AddCell(new PdfPCell(new Phrase(" ", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            doc.Add(anotacion);
            doc.Add(emptyTable);

            PdfPTable notacionMar = new PdfPTable(1);
            notacionMar.WidthPercentage = 100;

            notacionMar.AddCell(new PdfPCell(new Phrase(model.AnotacionMarginal, normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            doc.Add(notacionMar);

            doc.Add(new Paragraph(" ") { SpacingAfter = 1 });

            PdfPTable realizado = new PdfPTable(1);
            realizado.WidthPercentage = 100;

            realizado.AddCell(new PdfPCell(new Phrase($"Se extiende, firma y sella la presente en la Oficina Parroquial en Guatemala de la Asunción, {model.DiaExpedicion} de {model.MesExpedicion} del año {model.AnioExpedicion}", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            doc.Add(realizado);

            doc.Add(new Paragraph(" ") { SpacingAfter = 20 });

            doc.Add(new Paragraph($"{model.SacerdoteCat} {model.SacerdoteFirma}, FMM", subtitulo)
            {
                SpacingBefore = 100,
                Alignment = Element.ALIGN_CENTER
            });

            String tituloSacerdotal = "";

            if(model.tituloSacerdotal == "vicario")
            {
                tituloSacerdotal = "Vicario Parroquial";
            }

            if (model.tituloSacerdotal == "parroco")
            {
                tituloSacerdotal = "Párroco";
            }

            doc.Add(new Paragraph($"{tituloSacerdotal}", normal)
            {
                Alignment = Element.ALIGN_CENTER
            });
        }

        private void GenerarConstanciaPrimeraComunion(Document doc, CertificationModel model, iTextSharp.text.Font titulo, iTextSharp.text.Font subtitulo, iTextSharp.text.Font normal, iTextSharp.text.Font normalBold)
        {
            float padLeft = 15f;

            string? certi_configuracion = _configuracion["Certification:ViewText"];

            string certificacion = $"{certi_configuracion} {model.TipoSacramento}";

            doc.Add(new Paragraph(certificacion, subtitulo)
            {
                Alignment = Element.ALIGN_CENTER
            });

            PdfPCell emptyCell = new PdfPCell(new Phrase(" ", normalBold))
            {
                Border = Rectangle.NO_BORDER,
                FixedHeight = 5f
            };

            PdfPTable emptyTable = new PdfPTable(1);
            emptyTable.AddCell(emptyCell);

            doc.Add(new Paragraph(" ") { SpacingAfter = 3 });

            PdfPTable numPartidas = new PdfPTable(3);
            numPartidas.WidthPercentage = 100;
            numPartidas.SetWidths(new float[] { 33, 33, 33 });

            Phrase phraseNumero = new Phrase();
            phraseNumero.Add(new Chunk("No: ", normal));
            phraseNumero.Add(new Chunk(model.Numero!.ToString(), normalBold));

            Phrase phraseFolio = new Phrase();
            phraseFolio.Add(new Chunk("Folio: ", normal));
            phraseFolio.Add(new Chunk(model.Folio, normalBold));

            Phrase phrasePartida = new Phrase();
            phrasePartida.Add(new Chunk("Partida No: ", normal));
            phrasePartida.Add(new Chunk(model.Partida, normalBold));

            numPartidas.AddCell(new PdfPCell(phraseNumero)
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            numPartidas.AddCell(new PdfPCell(phraseFolio)
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            numPartidas.AddCell(new PdfPCell(phrasePartida)
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            doc.Add(numPartidas);
            doc.Add(new Paragraph(" ") { SpacingAfter = 3 });

            string nombrePersona = "Se encuentra inscrito (a):";

            PdfPTable nombre = new PdfPTable(2);
            nombre.WidthPercentage = 100;
            nombre.SetWidths(new float[] {35, 65 });

            nombre.AddCell(new PdfPCell(new Phrase(nombrePersona, normalBold))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            nombre.AddCell(new PdfPCell(new Phrase(model.NombreBautizado, normalBold))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(nombre);
            doc.Add(emptyTable);

            PdfPTable nacimiento = new PdfPTable(2);
            nacimiento.WidthPercentage = 100;
            nacimiento.SetWidths(new float[] {35, 65 });

            nacimiento.AddCell(new PdfPCell(new Phrase("Que nació:", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            nacimiento.AddCell(new PdfPCell(new Phrase(model.FechaNacimiento, normal))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(nacimiento);
            doc.Add(emptyTable);

            PdfPTable padre = new PdfPTable(2);
            padre.WidthPercentage = 100;
            padre.SetWidths(new float[] {35, 65 });

            padre.AddCell(new PdfPCell(new Phrase("Hijo(a) de:", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            padre.AddCell(new PdfPCell(new Phrase(model.NombrePadre, normal))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(padre);
            doc.Add(emptyTable);

            PdfPTable madre = new PdfPTable(2);
            madre.WidthPercentage = 100;
            madre.SetWidths(new float[] {35, 65 });

            madre.AddCell(new PdfPCell(new Phrase("Y de:", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            madre.AddCell(new PdfPCell(new Phrase(model.NombreMadre, normal))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(madre);
            doc.Add(emptyTable);

            PdfPTable FechaSacramento = new PdfPTable(2);
            FechaSacramento.WidthPercentage = 100;
            FechaSacramento.SetWidths(new float[] {35, 65 });

            FechaSacramento.AddCell(new PdfPCell(new Phrase($"Fecha de {model.TipoSacramento}:", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });
            FechaSacramento.AddCell(new PdfPCell(new Phrase($"{model.Dia} de {model.Mes} del año {model.Anio}", normal))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(FechaSacramento);
            doc.Add(emptyTable);

            PdfPTable anotacion = new PdfPTable(2);
            anotacion.WidthPercentage = 100;
            anotacion.SetWidths(new float[] { 35, 65 });

            anotacion.AddCell(new PdfPCell(new Phrase("En la anotacion marginal se lee:", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            anotacion.AddCell(new PdfPCell(new Phrase(" ", normal))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(anotacion);

            PdfPTable notacionMar = new PdfPTable(1);
            notacionMar.WidthPercentage = 100;

            notacionMar.AddCell(new PdfPCell(new Phrase(model.AnotacionMarginal, normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            doc.Add(notacionMar);

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

            doc.Add(new Paragraph($"{model.SacerdoteCat} {model.SacerdoteFirma}, FMM", subtitulo)
            {
                SpacingBefore = 100,
                Alignment = Element.ALIGN_CENTER
            });

            String tituloSacerdotal = "";

            if (model.tituloSacerdotal == "vicario")
            {
                tituloSacerdotal = "Vicario Parroquial";
            }

            if (model.tituloSacerdotal == "parroco")
            {
                tituloSacerdotal = "Párroco";
            }

            doc.Add(new Paragraph($"{tituloSacerdotal}", normal)
            {
                Alignment = Element.ALIGN_CENTER
            });
        }

        private void GenerarConstanciaConfirmacion(Document doc, CertificationModel model, iTextSharp.text.Font titulo, iTextSharp.text.Font subtitulo, iTextSharp.text.Font normal, iTextSharp.text.Font normalBold)
        {
            float padLeft = 15f;

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

            Phrase phraseNumero = new Phrase();
            phraseNumero.Add(new Chunk("No: ", normal));
            phraseNumero.Add(new Chunk(model.Numero!.ToString(), normalBold));

            Phrase phraseFolio = new Phrase();
            phraseFolio.Add(new Chunk("Folio: ", normal));
            phraseFolio.Add(new Chunk(model.Folio, normalBold));

            Phrase phrasePartida = new Phrase();
            phrasePartida.Add(new Chunk("Partida No: ", normal));
            phrasePartida.Add(new Chunk(model.Partida, normalBold));

            numPartidas.AddCell(new PdfPCell(phraseNumero)
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            numPartidas.AddCell(new PdfPCell(phraseFolio)
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            numPartidas.AddCell(new PdfPCell(phrasePartida)
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            doc.Add(numPartidas);
            doc.Add(new Paragraph(" ") { SpacingAfter = 3 });

            string nombrePersona = "Se encuentra inscrito (a):";

            PdfPTable nombre = new PdfPTable(2);
            nombre.WidthPercentage = 100;
            nombre.SetWidths(new float[] { 35, 65 });

            PdfPCell emptyCell = new PdfPCell(new Phrase(" ", normalBold))
            {
                Border = Rectangle.NO_BORDER,
                FixedHeight = 5f
            };

            PdfPTable emptyTable = new PdfPTable(1);
            emptyTable.AddCell(emptyCell);

            nombre.AddCell(new PdfPCell(new Phrase(nombrePersona, normalBold))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            nombre.AddCell(new PdfPCell(new Phrase(model.NombreBautizado, normalBold))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(nombre);
            doc.Add(emptyTable);

            PdfPTable FechaSacramento = new PdfPTable(2);
            FechaSacramento.WidthPercentage = 100;
            FechaSacramento.SetWidths(new float[] { 35, 65 });

            FechaSacramento.AddCell(new PdfPCell(new Phrase($"Fecha de {model.TipoSacramento}:", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });
            FechaSacramento.AddCell(new PdfPCell(new Phrase($"{model.Dia} de {model.Mes} del año {model.Anio}", normal))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(FechaSacramento);
            doc.Add(emptyTable);

            PdfPTable padre = new PdfPTable(2);
            padre.WidthPercentage = 100;
            padre.SetWidths(new float[] {35, 65 });

            padre.AddCell(new PdfPCell(new Phrase("Hijo(a) de:", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            padre.AddCell(new PdfPCell(new Phrase(model.NombrePadre, normal))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(padre);
            doc.Add(emptyTable);

            PdfPTable madre = new PdfPTable(2);
            madre.WidthPercentage = 100;
            madre.SetWidths(new float[] {35, 65 });

            madre.AddCell(new PdfPCell(new Phrase("Y de:", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            madre.AddCell(new PdfPCell(new Phrase(model.NombreMadre, normal))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(madre);
            doc.Add(emptyTable);

            PdfPTable padrinos = new PdfPTable(2);
            padrinos.WidthPercentage = 100;
            padrinos.SetWidths(new float[] {35, 65 });

            padrinos.AddCell(new PdfPCell(new Phrase("Padrinos:", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });
            padrinos.AddCell(new PdfPCell(new Phrase(model.NombrePadrinos?[0], normal))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(padrinos);
            doc.Add(emptyTable);

            PdfPTable madrina = new PdfPTable(2);
            madrina.WidthPercentage = 100;
            madrina.SetWidths(new float[] {35, 65 });

            madrina.AddCell(new PdfPCell(new Phrase("", normal))
            {
                Border = Rectangle.NO_BORDER
            });
            madrina.AddCell(new PdfPCell(new Phrase(model.NombrePadrinos?[1], normal))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(madrina);
            doc.Add(emptyTable);

            PdfPTable sacerdote = new PdfPTable(1);
            sacerdote.WidthPercentage = 100;
            sacerdote.SetWidths(new float[] { 100 });

            sacerdote.AddCell(new PdfPCell(new Phrase($"Quien fue confirmado(a) a los {model.edad} años de edad, por el Señor Obispo Mons. {model.NombreSacerdote}", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            doc.Add(sacerdote);
            doc.Add(emptyTable);

            PdfPTable anotacion = new PdfPTable(2);
            anotacion.WidthPercentage = 100;
            anotacion.SetWidths(new float[] {35, 65 });

            anotacion.AddCell(new PdfPCell(new Phrase("En la anotacion marginal se lee:", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            anotacion.AddCell(new PdfPCell(new Phrase(" ", normal))
            {
                Border = Rectangle.NO_BORDER
            });

            doc.Add(anotacion);
            doc.Add(emptyTable);

            PdfPTable notacionMar = new PdfPTable(1);
            notacionMar.WidthPercentage = 100;

            notacionMar.AddCell(new PdfPCell(new Phrase(model.AnotacionMarginal, normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            doc.Add(notacionMar);

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

            doc.Add(new Paragraph($"{model.SacerdoteCat} {model.SacerdoteFirma}, FMM", subtitulo)
            {
                SpacingBefore = 100,
                Alignment = Element.ALIGN_CENTER
            });

            String tituloSacerdotal = "";

            if (model.tituloSacerdotal == "vicario")
            {
                tituloSacerdotal = "Vicario Parroquial";
            }

            if (model.tituloSacerdotal == "parroco")
            {
                tituloSacerdotal = "Párroco";
            }

            doc.Add(new Paragraph($"{tituloSacerdotal}", normal)
            {
                Alignment = Element.ALIGN_CENTER
            });
        }

        private void GenerarConstanciaMatrimonio(Document doc, CertificationModel model, iTextSharp.text.Font titulo, iTextSharp.text.Font subtitulo, iTextSharp.text.Font normal, iTextSharp.text.Font normalBold)
        {
            if (string.IsNullOrEmpty(model.NombreBautizado) || string.IsNullOrEmpty(model.NombreEsposa))
            {
                // Lanza una excepción o devuelve un error
                throw new ArgumentException("Los nombres del esposo y la esposa son obligatorios para generar la constancia de matrimonio.");
            }

            float padLeft = 15f;

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

            Phrase phraseNumero = new Phrase();
            phraseNumero.Add(new Chunk("No: ", normal));
            phraseNumero.Add(new Chunk(model.Numero!.ToString(), normalBold));

            Phrase phraseFolio = new Phrase();
            phraseFolio.Add(new Chunk("Folio: ", normal));
            phraseFolio.Add(new Chunk(model.Folio, normalBold));

            Phrase phrasePartida = new Phrase();
            phrasePartida.Add(new Chunk("Partida No: ", normal));
            phrasePartida.Add(new Chunk(model.Partida, normalBold));

            numPartidas.AddCell(new PdfPCell(phraseNumero)
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            numPartidas.AddCell(new PdfPCell(phraseFolio)
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            numPartidas.AddCell(new PdfPCell(phrasePartida)
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            doc.Add(numPartidas);
            doc.Add(new Paragraph(" ") { SpacingAfter = 3 });

            PdfPCell emptyCell = new PdfPCell(new Phrase(" ", normalBold))
            {
                Border = Rectangle.NO_BORDER,
                FixedHeight = 5f
            };

            PdfPTable emptyTable = new PdfPTable(1);
            emptyTable.AddCell(emptyCell);

            string nombrePersona = "Se encuentra inscrito:";

            PdfPTable nombre = new PdfPTable(2);
            nombre.WidthPercentage = 100;
            nombre.SetWidths(new float[] {40, 60 });

            nombre.AddCell(new PdfPCell(new Phrase(nombrePersona, normalBold))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            nombre.AddCell(new PdfPCell(new Phrase(model.NombreBautizado, normalBold))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(nombre);
            doc.Add(emptyTable);

            PdfPTable padre = new PdfPTable(2);
            padre.WidthPercentage = 100;
            padre.SetWidths(new float[] { 40, 60 });

            padre.AddCell(new PdfPCell(new Phrase("Hijo de:", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            padre.AddCell(new PdfPCell(new Phrase(model.NombrePadre, normal))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(padre);
            doc.Add(emptyTable);

            PdfPTable madre = new PdfPTable(2);
            madre.WidthPercentage = 100;
            madre.SetWidths(new float[] { 40, 60 });

            madre.AddCell(new PdfPCell(new Phrase("Y de:", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            madre.AddCell(new PdfPCell(new Phrase(model.NombreMadre, normal))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(madre);
            doc.Add(emptyTable);

            string nombrePersona2 = "Quien contrajo Matrimonio con:";

            PdfPTable nombre2 = new PdfPTable(2);
            nombre2.WidthPercentage = 100;
            nombre2.SetWidths(new float[] { 40, 60 });

            nombre2.AddCell(new PdfPCell(new Phrase(nombrePersona2, normalBold))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            nombre2.AddCell(new PdfPCell(new Phrase(model.NombreEsposa, normalBold))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(nombre2);
            doc.Add(emptyTable);

            PdfPTable padre2 = new PdfPTable(2);
            padre2.WidthPercentage = 100;
            padre2.SetWidths(new float[] { 40, 60 });

            padre2.AddCell(new PdfPCell(new Phrase("Hija de:", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            padre2.AddCell(new PdfPCell(new Phrase(model.NombrePadreEsposa, normal))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(padre2);
            doc.Add(emptyTable);

            PdfPTable madre2 = new PdfPTable(2);
            madre2.WidthPercentage = 100;
            madre2.SetWidths(new float[] { 40, 60 });

            madre2.AddCell(new PdfPCell(new Phrase("Y de:", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            madre2.AddCell(new PdfPCell(new Phrase(model.NombreMadreEsposa, normal))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(madre2);
            doc.Add(emptyTable);

            PdfPTable fechaSacramento = new PdfPTable(1);
            fechaSacramento.WidthPercentage = 100;
            fechaSacramento.SetWidths(new float[] { 100 });

            fechaSacramento.AddCell(new PdfPCell(new Phrase($"El día {model.Dia} de {model.Mes} del año {model.Anio}", normalBold))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            doc.Add(fechaSacramento);
            doc.Add(emptyTable);

            PdfPTable padrinos = new PdfPTable(2);
            padrinos.WidthPercentage = 100;
            padrinos.SetWidths(new float[] { 40, 60 });

            padrinos.AddCell(new PdfPCell(new Phrase("Padrinos:", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });
            padrinos.AddCell(new PdfPCell(new Phrase(model.NombrePadrinos?[0], normal))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(padrinos);
            doc.Add(emptyTable);

            PdfPTable madrina = new PdfPTable(2);
            madrina.WidthPercentage = 100;
            madrina.SetWidths(new float[] { 40, 60 });

            madrina.AddCell(new PdfPCell(new Phrase("", normal))
            {
                Border = Rectangle.NO_BORDER
            });
            madrina.AddCell(new PdfPCell(new Phrase(model.NombrePadrinos?[1], normal))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(madrina);
            doc.Add(emptyTable);

            PdfPTable sacerdote = new PdfPTable(2);
            sacerdote.WidthPercentage = 100;
            sacerdote.SetWidths(new float[] { 40, 60 });

            sacerdote.AddCell(new PdfPCell(new Phrase("Celebro dicho Matrimonio:", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            sacerdote.AddCell(new PdfPCell(new Phrase(model.NombreSacerdote, normal))
            {
                Border = Rectangle.NO_BORDER,
                PaddingLeft = padLeft
            });

            doc.Add(sacerdote);
            doc.Add(emptyTable);

            PdfPTable anotacion = new PdfPTable(2);
            anotacion.WidthPercentage = 100;
            anotacion.SetWidths(new float[] { 35, 65 });

            anotacion.AddCell(new PdfPCell(new Phrase("En la anotacion marginal se lee:", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            anotacion.AddCell(new PdfPCell(new Phrase(" ", normal))
            {
                Border = Rectangle.NO_BORDER
            });

            doc.Add(anotacion);
            doc.Add(emptyTable);

            PdfPTable notacionMar = new PdfPTable(1);
            notacionMar.WidthPercentage = 100;

            notacionMar.AddCell(new PdfPCell(new Phrase(model.AnotacionMarginal, normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            doc.Add(notacionMar);

            doc.Add(new Paragraph(" ") { SpacingAfter = 3 });

            PdfPTable realizado = new PdfPTable(1);
            realizado.WidthPercentage = 100;

            realizado.AddCell(new PdfPCell(new Phrase($"Se extiende, firma y sella la presente en la Oficina Parroquial en Guatemala de la Asunción, {model.DiaExpedicion} de {model.MesExpedicion} del año {model.AnioExpedicion}", normal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            doc.Add(realizado);

            doc.Add(new Paragraph(" ") {  });

            doc.Add(new Paragraph($"{model.SacerdoteCat} {model.SacerdoteFirma}, FMM", subtitulo)
            {
                SpacingBefore = 70,
                Alignment = Element.ALIGN_CENTER
            });

            String tituloSacerdotal = "";

            if (model.tituloSacerdotal == "vicario")
            {
                tituloSacerdotal = "Vicario Parroquial";
            }

            if (model.tituloSacerdotal == "parroco")
            {
                tituloSacerdotal = "Párroco";
            }

            doc.Add(new Paragraph($"{tituloSacerdotal}", normal)
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
