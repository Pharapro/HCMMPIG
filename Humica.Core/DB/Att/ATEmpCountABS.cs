using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Humica.Core.DB
{
	[Table("ATEmpCountABS")]
	public class ATEmpCountABS
	{
		[Key]
		[StringLength(100)]
		public string DeptCode { get; set; }

		[StringLength(200)]
		public string DepartmentName { get; set; }

		public int Jan_ABS_Day { get; set; }
		public int Feb_ABS_Day { get; set; }
		public int Mar_ABS_Day { get; set; }
		public int Apr_ABS_Day { get; set; }
		public int May_ABS_Day { get; set; }
		public int Jun_ABS_Day { get; set; }
		public int Jul_ABS_Day { get; set; }
		public int Aug_ABS_Day { get; set; }
		public int Sep_ABS_Day { get; set; }
		public int Oct_ABS_Day { get; set; }
		public int Nov_ABS_Day { get; set; }
		public int Dec_ABS_Day { get; set; }

		public int Jan_Miss_Staff { get; set; }
		public int Feb_Miss_Staff { get; set; }
		public int Mar_Miss_Staff { get; set; }
		public int Apr_Miss_Staff { get; set; }
		public int May_Miss_Staff { get; set; }
		public int Jun_Miss_Staff { get; set; }
		public int Jul_Miss_Staff { get; set; }
		public int Aug_Miss_Staff { get; set; }
		public int Sep_Miss_Staff { get; set; }
		public int Oct_Miss_Staff { get; set; }
		public int Nov_Miss_Staff { get; set; }
		public int Dec_Miss_Staff { get; set; }
	}
}
