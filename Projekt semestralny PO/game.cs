//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Projekt_semestralny_PO
{
    using System;
    using System.Collections.Generic;
    
    public partial class game
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public game()
        {
            this.genres = new HashSet<genre>();
            this.platforms = new HashSet<platform>();
        }
    
        public int game_id { get; set; }
        public Nullable<int> producer_id { get; set; }
        public string game_title { get; set; }
        public short release_year { get; set; }
        public bool is_a_serie { get; set; }
    
        public virtual producer producer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<genre> genres { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<platform> platforms { get; set; }
    }
}
