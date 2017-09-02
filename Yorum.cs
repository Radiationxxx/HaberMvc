namespace HaberSitesi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Yorum")]
    public partial class Yorum
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string YorumBaslik { get; set; }

        [StringLength(50)]
        public string YorumIcerik { get; set; }

        public DateTime? YorumTarih { get; set; }

        public int? KullaniciId { get; set; }

        public int HaberId { get; set; }

        public virtual Haber Haber { get; set; }

        public virtual Kullanici Kullanici { get; set; }
    }
}
