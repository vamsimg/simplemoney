using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleMoney.Models
{
    


   //  [Table("BalanceSheets")]
     public class BalanceSheet
     {
     //     [Key]
     //     public int BalanceSheetID { get; set; }

     //     public int ApplicationID { get; set; }
     //     [ForeignKey("ApplicationID")]
     //     public Application Application { get; set; }
          public DateTime CreatedDate { get; set; }
          public string Notes { get; set; }

          public int EstimatedAssets { get; set; }
          public int EstimatedLiabilities { get; set; }

          public List<BalanceSheetEntry> BalanceSheetEntries { get; set; }
     }

     //[Table("BalanceSheetEntries")]
     public class BalanceSheetEntry
     {
      ////    [Key]
      //    public int BalanceSheetEntryID { get; set; }

      //   // [ForeignKey("BalanceSheetID")]
      //    public int BalanceSheetID { get; set; }

      //    public BalanceSheet BalanceSheet { get; set; }

          public DateTime CreatedDate { get; set; }
          public DateTime ReportDate { get; set; }

          public bool isAsset { get; set; }
          public decimal Amount { get; set; }
          public string Type { get; set; }
          public string Details { get; set; }
          public string AccountID { get; set; }
          public string Notes { get; set; }
     }

    // [Table("ProfitAndLossStatements")]
     public class ProfitAndLossStatement
     {
       // //  [Key]
       //   public int ProfitAndLossStatementID { get; set; }

       ////   [ForeignKey("ApplicationID")]
       //   public int ApplicationID { get; set; }

       //   public Application Application { get; set; }

          public DateTime FromDate { get; set; }
          public DateTime ToDate { get; set; }
                    
          public DateTime ReportDate { get; set; }

          public string Notes { get; set; }

          public int EstimatesIncome { get; set; }
          public int EstimatedExpenses { get; set; }

          public List<ProfitAndLossStatementEntry> ProfitAndLossStatementEntries { get; set; }
     }

  //   [Table("ProfitAndLossStatementEntries")]
     public class ProfitAndLossStatementEntry
     {
      ////    [Key]
      //    public int ProfitAndLossStatementEntryID { get; set; }

      // //   [ForeignKey("ProfitAndLossStatementID")]
      //    public int ProfitAndLossStatementID { get; set; }

      //    public ProfitAndLossStatement ProfitAndLossStatement { get; set; }

          public ProfitAndLossType ProfitAndLossType { get; set; }

          public string AccountID { get; set; }
          public string Details { get; set; }
          public string Notes { get; set; }
          public decimal Amount { get; set; }       
     }

     public enum ProfitAndLossType
     {
          Income,
          CostOfSales,
          Expense,
          OtherIncome,
          OtherExpense
     }    
}