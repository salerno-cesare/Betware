using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace ECMS.Helpers
{
    public static class Constants
    {
        //public static readonly int DEFAULT_SELECT_LIMIT = 200;
        //public static readonly int DEFAULT_ADMIN_RESULTS_LIMIT = 20;

        #region Result
        public static readonly IEnumerable<SelectListItem> BetResults = new List<SelectListItem>
        {
            new SelectListItem {Value = "0 - 0", Text="0 - 0"},
            new SelectListItem {Value = "0 - 1", Text="0 - 1"},
            new SelectListItem {Value = "1 - 0", Text="1 - 0"},
            new SelectListItem {Value = "0 - 2", Text="0 - 2"},
            new SelectListItem {Value = "1 - 1", Text="1 - 1"},
            new SelectListItem {Value = "2 - 0", Text="2 - 0"},
            new SelectListItem {Value = "0 - 3", Text="0 - 3"},
            new SelectListItem {Value = "1 - 2", Text="1 - 2"},
            new SelectListItem {Value = "2 - 1", Text="2 - 1"},
            new SelectListItem {Value = "3 - 0", Text="3 - 0"},
            new SelectListItem {Value = "0 - 4", Text="0 - 4"},
            new SelectListItem {Value = "1 - 3", Text="1 - 3"},
            new SelectListItem {Value = "2 - 2", Text="2 - 2"},
            new SelectListItem {Value = "3 - 1", Text="3 - 1"},
            new SelectListItem {Value = "4 - 0", Text="4 - 0"},
            new SelectListItem {Value = "0 - 5", Text="0 - 5"},
            new SelectListItem {Value = "1 - 4", Text="1 - 4"},
            new SelectListItem {Value = "2 - 3", Text="2 - 3"},
            new SelectListItem {Value = "3 - 2", Text="3 - 2"},
            new SelectListItem {Value = "4 - 1", Text="4 - 1"},
            new SelectListItem {Value = "5 - 0", Text="5 - 0"},
            new SelectListItem {Value = "0 - 6", Text="0 - 6"},
            new SelectListItem {Value = "1 - 5", Text="1 - 5"},
            new SelectListItem {Value = "2 - 4", Text="2 - 4"},
            new SelectListItem {Value = "3 - 3", Text="3 - 3"},
            new SelectListItem {Value = "4 - 2", Text="4 - 2"},
            new SelectListItem {Value = "5 - 1", Text="5 - 1"},
            new SelectListItem {Value = "6 - 0", Text="6 - 0"},
            new SelectListItem {Value = "0 - 7", Text="0 - 7"},
            new SelectListItem {Value = "1 - 6", Text="1 - 6"},
            new SelectListItem {Value = "2 - 5", Text="2 - 5"},
            new SelectListItem {Value = "3 - 4", Text="3 - 4"},
            new SelectListItem {Value = "4 - 3", Text="4 - 3"},
            new SelectListItem {Value = "5 - 2", Text="5 - 2"},
            new SelectListItem {Value = "6 - 1", Text="6 - 1"},
            new SelectListItem {Value = "7 - 0", Text="7 - 0"},
            new SelectListItem {Value = "0 - 8", Text="0 - 8"},
            new SelectListItem {Value = "1 - 7", Text="1 - 7"},
            new SelectListItem {Value = "2 - 6", Text="2 - 6"},
            new SelectListItem {Value = "3 - 5", Text="3 - 5"},
            new SelectListItem {Value = "4 - 4", Text="4 - 4"},
            new SelectListItem {Value = "5 - 3", Text="5 - 3"},
            new SelectListItem {Value = "6 - 2", Text="6 - 2"},
            new SelectListItem {Value = "7 - 1", Text="7 - 1"},
            new SelectListItem {Value = "8 - 0", Text="8 - 0"},
            new SelectListItem {Value = "1 - 8", Text="1 - 8"},
            new SelectListItem {Value = "2 - 7", Text="2 - 7"},
            new SelectListItem {Value = "3 - 6", Text="3 - 6"},
            new SelectListItem {Value = "4 - 5", Text="4 - 5"},
            new SelectListItem {Value = "5 - 4", Text="5 - 4"},
            new SelectListItem {Value = "6 - 3", Text="6 - 3"},
            new SelectListItem {Value = "7 - 2", Text="7 - 2"},
            new SelectListItem {Value = "8 - 1", Text="8 - 1"},
            new SelectListItem {Value = "2 - 8", Text="2 - 8"},
            new SelectListItem {Value = "3 - 7", Text="3 - 7"},
            new SelectListItem {Value = "4 - 6", Text="4 - 6"},
            new SelectListItem {Value = "5 - 5", Text="5 - 5"},
            new SelectListItem {Value = "6 - 4", Text="6 - 4"},
            new SelectListItem {Value = "7 - 3", Text="7 - 3"},
            new SelectListItem {Value = "8 - 2", Text="8 - 2"},
            new SelectListItem {Value = "3 - 8", Text="3 - 8"},
            new SelectListItem {Value = "4 - 7", Text="4 - 7"},
            new SelectListItem {Value = "5 - 6", Text="5 - 6"},
            new SelectListItem {Value = "6 - 5", Text="6 - 5"},
            new SelectListItem {Value = "7 - 4", Text="7 - 4"},
            new SelectListItem {Value = "8 - 3", Text="8 - 3"},
            new SelectListItem {Value = "4 - 8", Text="4 - 8"},
            new SelectListItem {Value = "5 - 7", Text="5 - 7"},
            new SelectListItem {Value = "6 - 6", Text="6 - 6"},
            new SelectListItem {Value = "7 - 5", Text="7 - 5"},
            new SelectListItem {Value = "8 - 4", Text="8 - 4"},
            new SelectListItem {Value = "5 - 8", Text="5 - 8"},
            new SelectListItem {Value = "6 - 7", Text="6 - 7"},
            new SelectListItem {Value = "7 - 6", Text="7 - 6"},
            new SelectListItem {Value = "8 - 5", Text="8 - 5"},
            new SelectListItem {Value = "6 - 8", Text="6 - 8"},
            new SelectListItem {Value = "7 - 7", Text="7 - 7"},
            new SelectListItem {Value = "8 - 6", Text="8 - 6"},
            new SelectListItem {Value = "7 - 8", Text="7 - 8"},
            new SelectListItem {Value = "8 - 7", Text="8 - 7"},
            new SelectListItem {Value = "8 - 8", Text="8 - 8"},


        };

        public static readonly IEnumerable<SelectListItem> BetTeams = new List<SelectListItem>
        {

            new SelectListItem{Value ="Germania", Text="Germania"},
            new SelectListItem{Value ="Scozia", Text="Scozia"},
            new SelectListItem{Value ="Ungheria", Text="Ungheria"},
            new SelectListItem{Value ="Svizzera", Text="Svizzera"},
            new SelectListItem{Value ="Spagna", Text="Spagna"},
            new SelectListItem{Value ="Croazia", Text="Croazia"},
            new SelectListItem{Value ="Italia", Text="Italia"},
            new SelectListItem{Value ="Albania", Text="Albania"},
            new SelectListItem{Value ="Slovenia", Text="Slovenia"},
            new SelectListItem{Value ="Danimarca", Text="Danimarca"},
            new SelectListItem{Value ="Serbia", Text="Serbia"},
            new SelectListItem{Value ="Inghilterra", Text="Inghilterra"},
            new SelectListItem{Value ="Polonia", Text="Polonia"},
            new SelectListItem{Value ="Paesi Bassi", Text="Paesi Bassi"},
            new SelectListItem{Value ="Austria", Text="Austria"},
            new SelectListItem{Value ="Francia", Text="Francia"},
            new SelectListItem{Value ="Belgio", Text="Belgio"},
            new SelectListItem{Value ="Slovacchia", Text="Slovacchia"},
            new SelectListItem{Value ="Romania", Text="Romania"},
            new SelectListItem{Value ="Ucraina", Text="Ucraina"},
            new SelectListItem{Value ="Turchia", Text="Turchia"},
            new SelectListItem{Value ="Georgia", Text="Georgia"},
            new SelectListItem{Value ="Portogallo", Text="Portogallo"},
            new SelectListItem{Value ="Repubblica Ceca", Text="Repubblica Ceca"},

        };

        #endregion

        public readonly static string QUARTI = "Quarti";
        public readonly static string SEMIFINALISTE = "Semifinali";
        public readonly static string FINALE = "Finale";
        public readonly static string WIN = "Win";

        public readonly static string FS_Q = "FS_Q";
        public readonly static string FS_S = "FS_S";
        public readonly static string FS_F = "FS_F";
        public readonly static string FS_W = "FS_W";


        public readonly static string Quarti_Descrizione = "Quali squadre disputeranno i Quarti? (FS_Q)";
        public readonly static string Semifinali_Descrizione = "Quali squadre disputeranno le Semifinali? (FS_S)";
        public readonly static string Finale_Descrizione = "Quali squadre disputeranno la Finale? (FS_F)";
        public readonly static string Win_Descrizione = "Quale squadra vincerà? (FS_W)";

        public readonly static DateTime EndRegistration = new DateTime(2024, 06, 19, 14, 00, 0);

        public readonly static DateTime EndFase1 = new DateTime(2024, 06, 14, 21, 00, 0);

        public readonly static DateTime EndFase2 = new DateTime(2024, 06, 19, 15, 0, 0);

        public readonly static DateTime EndFase3 = new DateTime(2024, 06, 23, 21, 0, 0);
    }
}