using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Constants
{
    public static class ErrorMessages
    {
        public static readonly string InvalidCui = "Cui-ul introdus nu este valid.";
        public static readonly string ExistingCui = "Exista deja o companie cu acest Cui.";
        public static readonly string ExistingWorkpoint = "Exista deja un punct de lucru cu acest nume si adresa.";
        public static readonly string MandatoryField = "Campul este obligatoriu.";
        public static readonly string DuplicatedProduct = "Au fost identificate produse duplicate:";
        public static readonly string InvalidFile = "Fisierul este invalid.";
        public static readonly string ExistingProduct = "Exista deja un produs cu acest nume.";
        public static readonly string InvalidStock = "Nu se poate adauga un stoc mai mic sau egal cu 0.";
        public static readonly string PdfGenerationFailed = "Eroare la generarea fisierului.";
    }
}
