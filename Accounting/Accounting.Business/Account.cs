﻿using Accounting.DataLayer.Context;
using Accounting.ViewModels.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Business
{
    public class Account
    {
        public static ReportViewModel ReportForMain()
        {
            ReportViewModel rp = new ReportViewModel();
            using (UnitOfWork db = new UnitOfWork())
            {
                DateTime starDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
                DateTime endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 30);

                var recive = db.AccountingRepository.Get(a => a.TypeID == 1 && a.DateTime > starDate && a.DateTime < endTime).Select(c => c.Amount).ToList();
                var pay = db.AccountingRepository.Get(a => a.TypeID == 2 && a.DateTime > starDate && a.DateTime < endTime).Select(c => c.Amount).ToList();

                rp.Recive = recive.Sum();
                rp.Pay = pay.Sum();
                rp.AccountBalance = recive.Sum() - pay.Sum();
            }
            return rp;
        }
    }
}


