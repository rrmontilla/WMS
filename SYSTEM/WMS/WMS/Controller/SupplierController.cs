using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WMS.Model;

namespace WMS.Controller
{
   public class SupplierController
    {
       wms_service.Service1 wms = new wms_service.Service1();
       public int insertSupplier(string xmlData)
       {
           int retval = 0;
          retval =  wms.InsertSupplier(xmlData);

           return retval;
       }
       public int updateSuplier(SupplierModel model)
       {
           int retval = 0;
           if (model.ID != 0)
           {
               retval = wms.UpdateSupplier(model.ID, model.SupplierCode, model.SupplierCodeTag, model.SupplierName, model.businessAddress, model.Tin, model.CellNumber, model.TelNumber, model.ContactPerson, model.ProductAvailed, model.PT, model.Country, model.SupplierCurrency);

           }
           else
           {
               retval = 3;
           }
           return retval;
       }

       public DataSet SelectSupplier()
       {
           DataSet ds = new DataSet();
           ds = wms.SelectSupplier();
           return ds;
       }

       public DataSet SelectSupplierByID(DataSet ds)
       {
           //DataSet ds = new DataSet();
           DataTable container = new DataTable();

           ds = wms.SelectSupplier();
           if (ds.Tables.Count > 0)
           {
               if (ds.Tables[0].Rows.Count > 0)
               { 
                   
               }
           }
           return ds;
       }
    }
}
