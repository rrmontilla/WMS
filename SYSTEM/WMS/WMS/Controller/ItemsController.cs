using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WMS.Model;

namespace WMS.Controller
{
    public class ItemsController
    {
        wms_service.Service1 wms = new wms_service.Service1();

        public int insertItem(string xmlData)
        {
           int retval = 0;
           retval =  wms.InsertItems(xmlData);
            return retval;
        }

        public DataSet selectItems()
        {
            DataSet ds = new DataSet();
            ds = wms.SelectAllItem();
            return ds;
        }

        public int updateItems(ItemModel model)
        {
            int retVal = 0;
            retVal = wms.UpdateItem(model.ID, model.itemCode, model.itemCodeTag, model.itemName, model.description, model.brand, model.unit, model.supplierName, model.ssLevel, model.LTDelivery, model.Inventory);
            return retVal;
        }

        public DataSet getItemByID(int itemID)
        {
            DataSet ds = new DataSet();
           ds= wms.SelectItemPerID(itemID);
            return ds;
        }
    }
}
