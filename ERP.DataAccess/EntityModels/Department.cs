﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.DataAccess.EntityModels
{
    [Table("department")]
    public partial class Department
    {
        public Department()
        {
            Employees = new HashSet<Employee>();
        }

        [Key]
        public int DepartmentId { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string DepartmentName { get; set; }
        public int? PreparedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PreparedDateTime { get; set; }
        public int? UpdatedByUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDateTime { get; set; }

        [ForeignKey(nameof(PreparedByUserId))]
        [InverseProperty(nameof(User.DepartmentPreparedByUsers))]
        public virtual User PreparedByUser { get; set; }
        [ForeignKey(nameof(UpdatedByUserId))]
        [InverseProperty(nameof(User.DepartmentUpdatedByUsers))]
        public virtual User UpdatedByUser { get; set; }
        [InverseProperty(nameof(Employee.Department))]
        public virtual ICollection<Employee> Employees { get; set; }
    }
}