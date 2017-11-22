
using System;
using APMS.DataAccess;
using System.Linq;
using System.Collections.Generic;
using APMS.Business.Dictionary;

namespace APMS.Business.API
{
    public class SaveSensorDataAPI : ISaveSensorDataAPI
    {

        private int GetSensorTypeBySensorCode(string sensorCode)
        {
            IRepository<APMS.DataAccess.SensorType> sensorTypeReposiotory = new Repository<APMS.DataAccess.SensorType>();
            var currentSensorType = sensorTypeReposiotory.GetAll().Where(x => x.SensorCode.Equals(sensorCode.Trim())).FirstOrDefault();
            if (currentSensorType != null)
            {
                return currentSensorType.SensorType1;
            }
            return -1;
        }
        public int GetWarningStateByValue(APMS.DataAccess.Record rec)
        {
            IRepository<APMS.DataAccess.Sensor> sensorReposiotory = new Repository<APMS.DataAccess.Sensor>();
            IRepository<APMS.DataAccess.Rule> ruleRepository = new Repository<APMS.DataAccess.Rule>();

            APMS.DataAccess.Sensor currentSensor = sensorReposiotory.GetAll().Where(x => x.SensorId.Equals(rec.SensorId)).FirstOrDefault();
            List<APMS.DataAccess.Rule> ruleList = ruleRepository.GetAll().Where(x => x.SensorId.Equals(rec.SensorId)).ToList();

            int currentSensorType = GetSensorTypeBySensorCode(currentSensor.SensorCode);
            int warningState = 0;


            if (currentSensor == null)
                return -1;

            foreach (var rule in ruleList)
            {
                if (currentSensorType == (int)Dictionary.SensorType.Voltage)
                {
                    if ((rule.OperatorType == (int)OperatorType.GreaterThan && rec.Value <= rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.GreaterThanOrEqual && rec.Value < rule.Value))
                    {
                        warningState = (int)WarningState.LowVolta;
                    }
                    if ((rule.OperatorType == (int)OperatorType.LessThan && rec.Value >= rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.LessThanOrEqual && rec.Value > rule.Value))
                    {
                        warningState = (int)WarningState.HighVolta;
                    }
                }
                else if (currentSensorType == (int)Dictionary.SensorType.Current)
                {
                    if ((rule.OperatorType == (int)OperatorType.GreaterThan && rec.Value <= rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.GreaterThanOrEqual && rec.Value < rule.Value))
                    {
                        warningState = (int)WarningState.Others;
                    }
                    if ((rule.OperatorType == (int)OperatorType.LessThan && rec.Value >= rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.LessThanOrEqual && rec.Value > rule.Value))
                    {
                        warningState = (int)WarningState.OverCurrent;
                    }
                }
                else if (currentSensorType == (int)Dictionary.SensorType.Temperature)
                {
                    if ((rule.OperatorType == (int)OperatorType.GreaterThan && rec.Value <= rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.GreaterThanOrEqual && rec.Value < rule.Value))
                    {
                        warningState = (int)WarningState.LowTemp;
                    }
                    if ((rule.OperatorType == (int)OperatorType.LessThan && rec.Value >= rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.LessThanOrEqual && rec.Value > rule.Value))
                    {
                        warningState = (int)WarningState.OverTemp;
                    }
                }
                else if (currentSensorType == (int)Dictionary.SensorType.Humidity)
                {
                    if ((rule.OperatorType == (int)OperatorType.GreaterThan && rec.Value <= rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.GreaterThanOrEqual && rec.Value < rule.Value))
                    {
                        warningState = (int)WarningState.TooDry;
                    }
                    if ((rule.OperatorType == (int)OperatorType.LessThan && rec.Value >= rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.LessThanOrEqual && rec.Value > rule.Value))
                    {
                        warningState = (int)WarningState.TooMoist;
                    }
                }
                else if (currentSensorType == (int)Dictionary.SensorType.Motion)
                {
                    if ((rule.OperatorType == (int)OperatorType.GreaterThan && rec.Value <= rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.GreaterThanOrEqual && rec.Value < rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.LessThan && rec.Value >= rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.LessThanOrEqual && rec.Value > rule.Value))
                    {
                        warningState = (int)WarningState.HasMotion;
                    }
                }
                else if (currentSensorType == (int)Dictionary.SensorType.Smoke)
                {
                    if ((rule.OperatorType == (int)OperatorType.GreaterThan && rec.Value <= rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.GreaterThanOrEqual && rec.Value < rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.LessThan && rec.Value >= rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.LessThanOrEqual && rec.Value > rule.Value))
                    {
                        warningState = (int)WarningState.HasSmoke;
                    }
                }
                else if (currentSensorType == (int)Dictionary.SensorType.Fire)
                {
                    if ((rule.OperatorType == (int)OperatorType.GreaterThan && rec.Value <= rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.GreaterThanOrEqual && rec.Value < rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.LessThan && rec.Value >= rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.LessThanOrEqual && rec.Value > rule.Value))
                    {
                        warningState = (int)WarningState.HasFire;
                    }
                }
            }

            return warningState;
            
        }

        public SaveSensorDataAPIViewModel SaveData(SaveSensorDataAPIViewModel model)
        {
            DateTime Now = DateTime.Now;

            IRepository<APMS.DataAccess.Device> deviceReposiotory = new Repository<APMS.DataAccess.Device>();
            IRepository<APMS.DataAccess.Sensor> sensorReposiotory = new Repository<APMS.DataAccess.Sensor>();
            IRepository<APMS.DataAccess.Record> recordReposiotory = new Repository<APMS.DataAccess.Record>();
            IRepository<APMS.DataAccess.Rule> ruleReposiotory = new Repository<APMS.DataAccess.Rule>();

            APMS.DataAccess.Device currentDevice = deviceReposiotory.GetAll().Where(x => x.DeviceId.Equals(model.DeviceId.Trim())).FirstOrDefault();
            List<APMS.DataAccess.Sensor> sensorList = sensorReposiotory.GetAll().ToList();
            if (currentDevice == null)
                return null;
            else
            {
                currentDevice.ActiveTime = Now;
                currentDevice.State = model.State;
                deviceReposiotory.Update(currentDevice);
            }

            for (int i=0; i<model.SensorParamList.Count;i++) 
            {
                var item = model.SensorParamList[i];
                var currentSensor = sensorList.Find(x => x.SensorId.Equals(item.SensorId.Trim()));
                if (currentSensor != null)
                {
                    var record = new APMS.DataAccess.Record();
                    record.SensorId = item.SensorId;
                    record.Value = (double)item.Value;
                    record.Time = Now;
                    record.State = model.State;
                    recordReposiotory.Insert(record);
                    int warningState = GetWarningStateByValue(record);
                    model.SensorParamList[i].WarningState = warningState;


                    if (warningState != currentSensor.WarningState)
                    {
                        currentSensor.WarningState = warningState;
                        if (warningState!=0)
                        {
                            Business.Web.Notification.SendWarningSensorStateNotification(currentSensor);
                        }
                    }
                    currentSensor.Value = item.Value;
                    sensorReposiotory.Update(currentSensor);
                }
            }

            return model;
        }
    }
}
