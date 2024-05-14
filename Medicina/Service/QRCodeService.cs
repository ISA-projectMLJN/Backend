using Medicina.Models;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using System;
using ZXing;
using System.Drawing;
using System.IO;

namespace Medicina.Service
{
    public class QRCodeService
    {

        public Bitmap GenerateQrCode(Reservation reservation)
        {
            /*Reservation reservation = new Reservation
            {
                Id = 1,
                UserId = 101,
                EquipmentId = 201,
                EquipmentCount = 2,
                IsCollected = false,
                Name = "John",
                Surname = "Doe",
                Deadline = DateTime.Now.AddDays(7)
            };*/

            string reservationJson = JsonConvert.SerializeObject(reservation);

            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 300,  // Set width of the QR code
                    Height = 300, // Set height of the QR code
                    Margin = 0    // Set margin of the QR code
                }
            };

            Bitmap qrBitmap = writer.Write(reservationJson);


            return qrBitmap;
        }
    }
}
