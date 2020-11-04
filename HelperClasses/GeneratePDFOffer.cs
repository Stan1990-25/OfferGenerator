using iText.Layout.Element;
using iText.Layout.Renderer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using System;
using System.Globalization;
using System.Linq;
using TMOffersClients.ViewModels;

namespace TMOffersClients.HelperClasses
{
    public class GeneratePDFOffer
    {
        private const string infoBeforeProductTable_1 = "Имаме удоволствието да Ви представим следната оферта за доставка на елементи ROBOTUNITS";
        private const string infoBeforeProductTable_2 = "по зададени от Вас размери, както следва:";

        private const string tableHeaders_1 = "Nr                Означение                                   Означение                        Кол.                   Ед.             Общо";
        private const string tableHeaders_2 = "Поз.           Номенклатура                          Наименование                    Броя                Цена             Лева";

        private const string deliveryNote_1 = "Срокът на доставка е ориентировъчен. При поръчка, осведомете се за срока на доставка,";
        private const string deliveryNote_2 = "съобразен с текущата ни производствена програма";
        private const string deliveryNote_3 = "Моля, при поръчка посочвайте номера на офертата.";


        public static void CreatePDF(OfferViewModel offer, IWebHostEnvironment env)
        {
            PdfDocument doc = new PdfDocument();

            int numberOfPages = (offer.SelectedProducts.Count / 25) + 1;
            int prodCounter = 0;
            int itemCounter = 0;

            using (doc)
            {
                XFont fontOffer = GenerateFont("Arial", 12, XFontStyle.Bold);
                XFont fontClientAndTerms = GenerateFont("Arial", 10, XFontStyle.Bold);
                XFont fontProducts = GenerateFont("Arial", 10, XFontStyle.Regular);

                for (int i = 0; i < numberOfPages; i++)
                {
                    XGraphics gfx = GeneratePage(doc, env);

                    using (gfx)
                    {
                        if (i == 0)
                        {
                            // Offer title
                            GenerateOfferData(gfx, fontOffer, offer);
                            GenreateClientData(gfx, fontClientAndTerms, offer);
                        }
                        
                        double tableRowOffset = 320;
                        if (i > 0)
                            tableRowOffset = 130;
                        // Table headers
                        tableRowOffset = GenerateTableHeaders(gfx, fontClientAndTerms, tableRowOffset);

                        tableRowOffset += 20;
                        // Products
                        int remainingItems = offer.SelectedProducts.Count - prodCounter;
                        for (int k = prodCounter; k < offer.SelectedProducts.Count; k++)
                        {
                            tableRowOffset = GenerateProductInTable(gfx, fontProducts, offer.SelectedProducts[k], prodCounter, tableRowOffset);

                            tableRowOffset += 12;

                            prodCounter++;
                            itemCounter++;

                            if (i == 0 && itemCounter == 25 && remainingItems > 25)
                                break;
                            else if (i > 0 && itemCounter == 40 && remainingItems > 40)
                                break;
                            else if (i > 0 && itemCounter == 25 && remainingItems <= 40)
                                break;
                        }

                        tableRowOffset += 5;

                        gfx.DrawRectangle(XPens.Black, XBrushes.Black, 50, tableRowOffset, 495, 2);

                        if (i == numberOfPages - 1 || offer.SelectedProducts.Count == prodCounter)
                        {
                            tableRowOffset = GenerateDeliveryTerms(gfx, fontClientAndTerms, offer, env, tableRowOffset);

                            if (offer.SelectedProducts.Count == prodCounter)
                            {
                                gfx.DrawString((i + 1).ToString(), fontProducts, XBrushes.Black, new XRect(50, 770, 120, 14), XStringFormats.CenterLeft);
                                break;
                            }
                        }

                        itemCounter = 0;

                        gfx.DrawString((i + 1).ToString(), fontProducts, XBrushes.Black, new XRect(50, 770, 120, 14), XStringFormats.CenterLeft);
                    }
                }

                var fileName = offer.Number.Replace("/", "%2F");

                doc.Save("E:\\Generated Offers\\" + fileName + ".pdf");
            }
        }

        private static XGraphics GeneratePage(PdfDocument document, IWebHostEnvironment env)
        {
            var page = document.AddPage();
            page.Size = PageSize.A4;
            page.Orientation = PageOrientation.Portrait;
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Header
            XImage headerImg = XImage.FromFile(env.WebRootPath + "\\images\\offer_form_header.png");
            gfx.DrawImage(headerImg, new XPoint(50, 80));

            return gfx;
        }

        private static XFont GenerateFont(string fontName, double size, XFontStyle style)
        {
            return new XFont(fontName, size, style);
        }       

        private static void GenerateOfferData(XGraphics gfx, XFont fontOffer, OfferViewModel offer)
        {
            gfx.DrawString("ОФЕРТА", fontOffer, XBrushes.Black, new XRect(247.5, 130, 100, 60), XStringFormats.Center);
            gfx.DrawString($"{offer.Number}", fontOffer, XBrushes.Black, new XRect(247.5, 144, 100, 60), XStringFormats.Center);
        }

        private static void GenreateClientData(XGraphics gfx, XFont fontClientAndTerms, OfferViewModel offer)
        {
            // Cleint Name and Date
            XRect clNameAndDate = new XRect(50, 210, 495, 14);
            gfx.DrawString($"До: {offer.ClientName}", fontClientAndTerms, XBrushes.Black, clNameAndDate, XStringFormats.CenterLeft);
            gfx.DrawString($"Дата: {DateTime.Now.ToString("dd/MM/yyyy")}г.", fontClientAndTerms, XBrushes.Black, clNameAndDate, XStringFormats.CenterRight);
            // Client City and Phone number
            gfx.DrawString($"гр.: {offer.ClientCity}", fontClientAndTerms, XBrushes.Black, new XRect(50, 222, 495, 14), XStringFormats.CenterLeft);
            gfx.DrawString($"тел.: {offer.ClientPhoneNumber}", fontClientAndTerms, XBrushes.Black, new XRect(50, 234, 495, 14), XStringFormats.CenterLeft);
            // Introduction before the product table
            gfx.DrawString($"На вниманието на: {offer.ContactName}", fontClientAndTerms, XBrushes.Black, new XRect(100, 246, 395, 14), XStringFormats.CenterLeft);
            gfx.DrawString($"Уважаеми г-н/г-жа {offer.ContactName/*.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]*/}", fontClientAndTerms, XBrushes.Black, new XRect(50, 270, 495, 14), XStringFormats.CenterLeft);
            gfx.DrawString(infoBeforeProductTable_1, fontClientAndTerms, XBrushes.Black, new XRect(55, 282, 490, 14), XStringFormats.Center);
            gfx.DrawString(infoBeforeProductTable_2, fontClientAndTerms, XBrushes.Black, new XRect(55, 294, 490, 14), XStringFormats.Center);
        }

        private static double GenerateTableHeaders(XGraphics gfx, XFont fontClientAndTerms, double tableRowOffset)
        {
            gfx.DrawRectangle(XPens.Black, XBrushes.Black, 50, tableRowOffset, 495, 2);
            gfx.DrawString(tableHeaders_1, fontClientAndTerms, XBrushes.Black, new XRect(52, tableRowOffset += 5, 495, 14), XStringFormats.CenterLeft);
            gfx.DrawString(tableHeaders_2, fontClientAndTerms, XBrushes.Black, new XRect(52, tableRowOffset += 12, 495, 14), XStringFormats.CenterLeft);
            gfx.DrawRectangle(XPens.Black, XBrushes.Black, 50, tableRowOffset += 13, 495, 2);

            return tableRowOffset;
        }

        private static double GenerateProductInTable(XGraphics gfx, XFont fontProducts, OrderProduct product, int prodCounter, double tableRowOffset)
        {
            gfx.DrawString((prodCounter + 1).ToString(), fontProducts, XBrushes.Black, new XRect(45, tableRowOffset, 28, 14), XStringFormats.Center);
            gfx.DrawString(product.ProductNumber, fontProducts, XBrushes.Black, new XRect(90, tableRowOffset, 120, 14), XStringFormats.CenterLeft);
            gfx.DrawString(product.Description, fontProducts, XBrushes.Black, new XRect(210, tableRowOffset, 190, 14), XStringFormats.CenterLeft);

            if (product.Category == "Профили")
                gfx.DrawString(product.Quantity.ToString() + "/" + product.Meters + "м.", fontProducts, XBrushes.Black, new XRect(370, tableRowOffset, 50, 14), XStringFormats.Center);
            else
                gfx.DrawString(product.Quantity.ToString(), fontProducts, XBrushes.Black, new XRect(366, tableRowOffset, 50, 14), XStringFormats.Center);

            gfx.DrawString(product.UnitPrice.ToString("C2"), fontProducts, XBrushes.Black, new XRect(435, tableRowOffset, 50, 14), XStringFormats.Center);
            gfx.DrawString(product.TotalPrice.ToString("C2"), fontProducts, XBrushes.Black, new XRect(495, tableRowOffset, 50, 14), XStringFormats.Center);

            return tableRowOffset;
        }

        private static double GenerateDeliveryTerms(XGraphics gfx, XFont fontClientAndTerms, OfferViewModel offer, IWebHostEnvironment env, double tableRowOffset)
        {
            // Total + Discount + VAT
            gfx.DrawString("Сума без ДДС:", fontClientAndTerms, XBrushes.Black, new XRect(50, tableRowOffset += 2, 420, 14), XStringFormats.CenterRight);
            gfx.DrawString((offer.SelectedProducts.Sum(op => op.TotalPrice)).ToString("C2", CultureInfo.CurrentCulture), fontClientAndTerms, XBrushes.Black, new XRect(495, tableRowOffset, 50, 14), XStringFormats.CenterRight);
            gfx.DrawString($"Отстъпка: {offer.Discount}%", fontClientAndTerms, XBrushes.Black, new XRect(50, tableRowOffset += 12, 420, 14), XStringFormats.CenterRight);
            gfx.DrawString((offer.SelectedProducts.Sum(op => op.TotalPrice) * (decimal)(offer.Discount / 100)).ToString("C2", CultureInfo.CurrentCulture), fontClientAndTerms, XBrushes.Black, new XRect(495, tableRowOffset, 50, 14), XStringFormats.CenterRight);
            gfx.DrawString($"Сума с отстъпка без ДДС:", fontClientAndTerms, XBrushes.Black, new XRect(50, tableRowOffset += 12, 420, 14), XStringFormats.CenterRight);
            gfx.DrawString(((offer.SelectedProducts.Sum(op => op.TotalPrice) - offer.SelectedProducts.Sum(op => op.TotalPrice) * (decimal)(offer.Discount / 100))).ToString("C2", CultureInfo.CurrentCulture), fontClientAndTerms, XBrushes.Black, new XRect(495, tableRowOffset, 50, 14), XStringFormats.CenterRight);
            gfx.DrawString($"20% ДДС:", fontClientAndTerms, XBrushes.Black, new XRect(50, tableRowOffset += 12, 420, 14), XStringFormats.CenterRight);
            gfx.DrawString((0.2M * (offer.SelectedProducts.Sum(op => op.TotalPrice) - offer.SelectedProducts.Sum(op => op.TotalPrice) * (decimal)(offer.Discount / 100))).ToString("C2", CultureInfo.CurrentCulture), fontClientAndTerms, XBrushes.Black, new XRect(495, tableRowOffset, 50, 14), XStringFormats.CenterRight);
            gfx.DrawString($"Общо с ДДС:", fontClientAndTerms, XBrushes.Black, new XRect(50, tableRowOffset += 12, 420, 14), XStringFormats.CenterRight);
            gfx.DrawString((1.2M * (offer.SelectedProducts.Sum(op => op.TotalPrice) - offer.SelectedProducts.Sum(op => op.TotalPrice) * (decimal)(offer.Discount / 100))).ToString("C2", CultureInfo.CurrentCulture), fontClientAndTerms, XBrushes.Black, new XRect(495, tableRowOffset, 50, 14), XStringFormats.CenterRight);

            gfx.DrawRectangle(XPens.Black, XBrushes.Black, 50, tableRowOffset += 14, 495, 2);

            // Delivery Terms
            gfx.DrawString($"Валидност на офертата: {offer.ExpirationDays.ToString()} работни дни", fontClientAndTerms, XBrushes.Black, new XRect(50, tableRowOffset += 10, 495, 14), XStringFormats.CenterLeft);
            gfx.DrawString($"Условия на доставката: {offer.DeliveryTerms}", fontClientAndTerms, XBrushes.Black, new XRect(50, tableRowOffset += 12, 495, 14), XStringFormats.CenterLeft);
            gfx.DrawString($"Начин на плащане: {offer.PaymentType}", fontClientAndTerms, XBrushes.Black, new XRect(50, tableRowOffset += 12, 495, 14), XStringFormats.CenterLeft);
            gfx.DrawString($"Срок на доставката: {offer.DeliveryDeadline}", fontClientAndTerms, XBrushes.Black, new XRect(50, tableRowOffset += 12, 495, 14), XStringFormats.CenterLeft);
            gfx.DrawString($"Начин на доставка: {offer.DeliveryType}", fontClientAndTerms, XBrushes.Black, new XRect(50, tableRowOffset += 12, 495, 14), XStringFormats.CenterLeft);

            gfx.DrawString(deliveryNote_1, fontClientAndTerms, XBrushes.Black, new XRect(50, tableRowOffset += 20, 495, 14), XStringFormats.CenterLeft);
            gfx.DrawString(deliveryNote_2, fontClientAndTerms, XBrushes.Black, new XRect(50, tableRowOffset += 12, 495, 14), XStringFormats.CenterLeft);
            gfx.DrawString(deliveryNote_3, fontClientAndTerms, XBrushes.Black, new XRect(50, tableRowOffset += 20, 495, 14), XStringFormats.CenterLeft);
            gfx.DrawString($"С уважение: {offer.Author}", fontClientAndTerms, XBrushes.Black, new XRect(50, tableRowOffset += 20, 495, 14), XStringFormats.CenterLeft);

            // Footer
            XImage footerImg = XImage.FromFile(env.WebRootPath + "\\images\\robotunits.png");
            gfx.DrawImage(footerImg, 420, 751.2, 150, 50);

            return tableRowOffset;
        }
    }
}
