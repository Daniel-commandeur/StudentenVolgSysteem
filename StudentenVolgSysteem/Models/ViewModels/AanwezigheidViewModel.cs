using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentenVolgSysteem.Models.ViewModels
{
    public class AanwezigheidViewModel
    {
        public IEnumerable<AanwezigheidOptionModel> AanwezigheidsOptions { get; set; }
        public List<PresentieEntryRow> PresentieEntryRows { get; set; }
    }

    public class PresentieEntryRow
    {
        public List<PresentieEntryModel> PresentieEntrys { get; set; }
    }
}