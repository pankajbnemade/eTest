﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.DataAccess.EntityModels
{
    [Table("chargetype")]
    public partial class Chargetype
    {
        [Key]
        public int ChargeTypeId { get; set; }
        [Required]
        [Column(TypeName = "varchar(250)")]
        public string ChargeTypeName { get; set; }
        public int? PreparedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PreparedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDateTime { get; set; }

        [ForeignKey(nameof(PreparedByUserId))]
        [InverseProperty(nameof(Aspnetuser.ChargetypePreparedByUsers))]
        public virtual Aspnetuser PreparedByUser { get; set; }
        [ForeignKey(nameof(UpdatedByUserId))]
        [InverseProperty(nameof(Aspnetuser.ChargetypeUpdatedByUsers))]
        public virtual Aspnetuser UpdatedByUser { get; set; }
    }
}