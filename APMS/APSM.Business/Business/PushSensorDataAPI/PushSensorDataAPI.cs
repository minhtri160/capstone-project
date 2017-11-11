
using System;
using APMS.DataAccess;
using System.Linq;
using System.Collections.Generic;

namespace APMS.Business.API
{
    public class PushSensorDataAPI : IPushSensorDataAPI
    {
        private void SetFlag(APMS.DataAccess.Record rec, bool flag)
        {
            IRepository<APMS.DataAccess.Flag> flagReposiotory = new Repository<APMS.DataAccess.Flag>();

            var currentFlag = flagReposiotory.GetAll().Where(x => x.DeviceId.Equals(rec.DeviceId.Trim()) && x.SensorId.Equals(rec.SensorId.Trim())).FirstOrDefault();
            if (currentFlag == null)
            {
                currentFlag = new APMS.DataAccess.Flag();
                currentFlag.DeviceId = rec.DeviceId.Trim();
                currentFlag.SensorId = rec.SensorId.Trim();
                currentFlag.State = (int)rec.State;
                currentFlag.Flag1 = flag;
                flagReposiotory.Insert(currentFlag);
            }
            else
            {
                if (currentFlag.State != rec.State || currentFlag.Flag1 != flag)
                {
                    currentFlag.State = (int)rec.State;
                    currentFlag.Flag1 = flag;
                    flagReposiotory.Update(currentFlag);
                }
            }
        }

        private void CheckRuleData(APMS.DataAccess.Record rec)
        {
            IRepository<APMS.DataAccess.Rule> ruleReposiotory = new Repository<APMS.DataAccess.Rule>();

            List<APMS.DataAccess.Rule> ruleList = ruleReposiotory.GetAll().Where(x => x.SensorId.Equals(rec.SensorId.Trim()) && x.State == rec.State).ToList();
            bool flag = false;
            foreach (var item in ruleList)
            {
                if (item.OperatorType == 1 && rec.Value < item.Value)  // OperatorType 1 = ">="
                {
                    flag = true;
                    // notification
                    SetFlag(rec, true);
                }
                else if (item.OperatorType == 2 && rec.Value <= item.Value)  // OperatorType 2 = ">"
                {
                    flag = true;
                    // notification
                    SetFlag(rec, true);
                }
                else if (item.OperatorType == 3 && rec.Value > item.Value) // OperatorType 3 = "<="
                {
                    flag = true;
                    // notification
                    SetFlag(rec, true);
                }
                else if (item.OperatorType == 4 && rec.Value >= item.Value) //OperatorType 4 = "<"
                {
                    flag = true;
                    // notification
                    SetFlag(rec, true);
                }
            }
            if (!flag)
            {
                SetFlag(rec, false);
            }
        }

        public bool SaveData(PushSensorDataAPIViewModel model)
        {
            DateTime Now = DateTime.Now;

            IRepository<APMS.DataAccess.Device> deviceReposiotory = new Repository<APMS.DataAccess.Device>();
            IRepository<APMS.DataAccess.Sensor> sensorReposiotory = new Repository<APMS.DataAccess.Sensor>();
            IRepository<APMS.DataAccess.Record> recordReposiotory = new Repository<APMS.DataAccess.Record>();
            IRepository<APMS.DataAccess.Rule> ruleReposiotory = new Repository<APMS.DataAccess.Rule>();
            IRepository<APMS.DataAccess.Flag> flagReposiotory = new Repository<APMS.DataAccess.Flag>();

            APMS.DataAccess.Device currentDevice = deviceReposiotory.GetAll().Where(x => x.DeviceId.Equals(model.DeviceId.Trim())).FirstOrDefault();
            if (currentDevice == null)
                return false;
            else
            {
                currentDevice.UpdateTime = Now;
                currentDevice.State = model.State;
                deviceReposiotory.Update(currentDevice);
            }

            foreach (var item in model.SensorParamList)
            {
                var currentSensor = sensorReposiotory.GetAll().Where(x => x.SensorId.Equals(item.SensorId.Trim())).FirstOrDefault();
                if (currentSensor != null)
                {
                    currentSensor.Value = item.Value;
                    sensorReposiotory.Update(currentSensor);

                    var record = new APMS.DataAccess.Record();
                    record.DeviceId = model.DeviceId;
                    record.SensorId = item.SensorId;
                    record.Value = (double)item.Value;
                    record.Time = Now;
                    record.State = model.State;
                    recordReposiotory.Insert(record);
                    CheckRuleData(record);
                }
            }

            return true;
        }
    }
}
