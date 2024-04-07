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
        public static readonly string ExistingCompanyName = "Exista deja o companie cu acest nume.";

        public static readonly string InvalidData = "A aparut o eroare.";

        public static readonly string ExistingWorkpoint = "Exista deja un punct de lucru cu acest nume si adresa.";
        public static readonly string MandatoryField = "Campul este obligatoriu.";
       
        public static readonly string DuplicatedProduct = "Au fost identificate produse duplicate. Te rog sa revizuiesti comanda";
        public static readonly string EmptyProductList = "Nu ai selectat niciun produs pentru comanda.";
        public static readonly string InvalidQuantity = "Cantitatea setata pentru unul sau mai multe produse este invalida.";
        public static readonly string OrderStatusError = "Statusul comenzilor procesate nu mai poate fi modificat.";

        public static readonly string ExistingProduct = "Exista deja un produs cu acest nume.";
        public static readonly string InvalidStock = "Nu se poate adauga un stoc mai mic sau egal cu 0.";
        public static readonly string InvalidPrice = "Nu se poate adauga un produs cu un pret mai mic sau egal cu 0 RON.";
        public static readonly string PdfGenerationFailed = "Eroare la generarea fisierului.";
        public static readonly string InvalidFileExtension = "Fisierul trebuie sa fie de tip .csv";
        public static readonly string InvalidFile = "Fisierul este invalid. Verifica continutul fisierului.";
        public static readonly string AllFieldsAreMandatory = "Toate campurile sunt obligatorii.";

        public static readonly string ExistingUsername = "Exista deja un client cu acest usernume.";
        public static readonly string InvalidCompany = "Nu ai selectat compania noului user. Daca este o companie noua, o poti crea din tab-ul Companii.";
        public static readonly string InvalidWorkpoint = "Nu ai selectat punctul de lucru pentru noua comanda. Daca este un punct de lucru nou, il poti crea din tab-ul Puncte de lucru.";
    }
}
