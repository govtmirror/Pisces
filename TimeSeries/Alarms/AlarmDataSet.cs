﻿using System;
using System.Data;
namespace Reclamation.TimeSeries.Alarms
{
}
namespace Reclamation.TimeSeries.Alarms {
    
    
    public partial class AlarmDataSet {
        private Core.BasicDBServer m_server;

        public Core.BasicDBServer Server
        {
            get { return m_server; }
            set { m_server = value; }
        }


        public void SaveTable(DataTable table)
        {
            m_server.SaveTable(table);
        }
        
        /// <summary>
        /// Gets a list of alarms in priority order for processing
        /// only alarms with status (new, or unconfirmed)
        /// </summary>
        /// <returns></returns>
        public alarm_phone_queueDataTable GetNewAlarms()
        {
            string sql = "select * from alarm_phone_queue where status='new' or status = 'unconfirmed' order by priority";
            m_server.FillTable(alarm_phone_queue, sql);
            return alarm_phone_queue;
        }


       
    }
    
}



