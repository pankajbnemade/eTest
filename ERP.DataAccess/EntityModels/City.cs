﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.DataAccess.EntityModels
{
    [Table("city")]
    public partial class City
    {
        public City()
        {
            Ledgeraddresses = new HashSet<Ledgeraddress>();
        }

        [Key]
        public int CityId { get; set; }
        [Required]
        [Column(TypeName = "varchar(500)")]
        public string CityName { get; set; }
        public int StateId { get; set; }
        public int? PreparedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PreparedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDateTime { get; set; }

        [ForeignKey(nameof(PreparedByUserId))]
        [InverseProperty(nameof(User.CityPreparedByUsers))]
        public virtual User PreparedByUser { get; set; }
        [ForeignKey(nameof(StateId))]
        [InverseProperty("Cities")]
        public virtual State State { get; set; }
        [ForeignKey(nameof(UpdatedByUserId))]
        [InverseProperty(nameof(User.CityUpdatedByUsers))]
        public virtual User UpdatedByUser { get; set; }
        [InverseProperty(nameof(Ledgeraddress.City))]
        public virtual ICollection<Ledgeraddress> Ledgeraddresses { get; set; }
    }
}