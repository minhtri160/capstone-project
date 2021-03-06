﻿
using System;
using System.Linq;
using System.Collections.Generic;
using APMS.DataAccess;
using APMS.Business.Dictionary;

namespace APMS.Business.Web
{
    public class SensorBusiness : ISensorBusiness
    {
        private IRepository<DataAccess.SensorType> sensorTypeReposiotory = new Repository<DataAccess.SensorType>();
        private IRepository<Sensor> sensorReposiotory = new Repository<Sensor>();
        private IRepository<Device> deviceReposiotory = new Repository<Device>();
        private IRepository<Record> recordReposiotory = new Repository<Record>();
        private IRepository<Rule> ruleReposiotory = new Repository<Rule>();
        private IRepository<Rule> ruleRepository = new Repository<Rule>();

        private int GetSensorTypeBySensorCode(string sensorCode)
        {
            var currentSensorType = sensorTypeReposiotory.GetAll().Where(x => x.SensorCode.Equals(sensorCode.Trim())).FirstOrDefault();
            if (currentSensorType != null)
            {
                return currentSensorType.SensorType1;
            }
            return -1;
        }
        private int? GetWarningStateByValue(Record rec, int deviceState)
        {
            Sensor currentSensor = sensorReposiotory.GetAll().Where(x => x.SensorId.Equals(rec.SensorId)).FirstOrDefault();
            List<Rule> ruleList = ruleRepository.GetAll().Where(x => x.SensorId.Equals(rec.SensorId) && x.State == deviceState).ToList();

            int currentSensorType = GetSensorTypeBySensorCode(currentSensor.SensorCode);
            int warningState = 0;


            if (currentSensor == null)
                return null;

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
                else if (currentSensorType == (int)Dictionary.SensorType.Spin)
                {
                    if ((rule.OperatorType == (int)OperatorType.GreaterThan && rec.Value <= rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.GreaterThanOrEqual && rec.Value < rule.Value))
                    {
                        warningState = (int)WarningState.FanSlow;
                    }
                    if ((rule.OperatorType == (int)OperatorType.LessThan && rec.Value >= rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.LessThanOrEqual && rec.Value > rule.Value))
                    {
                        warningState = (int)WarningState.FanFast;
                    }
                }
                else if (currentSensorType == (int)Dictionary.SensorType.Light)
                {
                    if ((rule.OperatorType == (int)OperatorType.GreaterThan && rec.Value <= rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.GreaterThanOrEqual && rec.Value < rule.Value))
                    {
                        warningState = (int)WarningState.LampNotWork;
                    }
                    if ((rule.OperatorType == (int)OperatorType.LessThan && rec.Value >= rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.LessThanOrEqual && rec.Value > rule.Value))
                    {
                        warningState = (int)WarningState.Others;
                    }
                }
                else if (currentSensorType == (int)Dictionary.SensorType.WaterLeak)
                {
                    if ((rule.OperatorType == (int)OperatorType.GreaterThan && rec.Value <= rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.GreaterThanOrEqual && rec.Value < rule.Value))
                    {
                        warningState = (int)WarningState.Others;
                    }
                    if ((rule.OperatorType == (int)OperatorType.LessThan && rec.Value >= rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.LessThanOrEqual && rec.Value > rule.Value))
                    {
                        warningState = (int)WarningState.HasWaterLeak;
                    }
                }
                else if (currentSensorType == (int)Dictionary.SensorType.Electric)
                {
                    if ((rule.OperatorType == (int)OperatorType.GreaterThan && rec.Value <= rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.GreaterThanOrEqual && rec.Value < rule.Value))
                    {
                        warningState = (int)WarningState.PowerOutage;
                    }
                    if ((rule.OperatorType == (int)OperatorType.LessThan && rec.Value >= rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.LessThanOrEqual && rec.Value > rule.Value))
                    {
                        warningState = (int)WarningState.StillPowerSupply;
                    }
                }
                else if (currentSensorType == (int)Dictionary.SensorType.Power)
                {
                    if ((rule.OperatorType == (int)OperatorType.GreaterThan && rec.Value <= rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.GreaterThanOrEqual && rec.Value < rule.Value))
                    {
                        warningState = (int)WarningState.Others;
                    }
                    if ((rule.OperatorType == (int)OperatorType.LessThan && rec.Value >= rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.LessThanOrEqual && rec.Value > rule.Value))
                    {
                        warningState = (int)WarningState.OverPower;
                    }
                }
                else if (currentSensorType == (int)Dictionary.SensorType.Gas)
                {
                    if ((rule.OperatorType == (int)OperatorType.GreaterThan && rec.Value <= rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.GreaterThanOrEqual && rec.Value < rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.LessThan && rec.Value >= rule.Value) ||
                        (rule.OperatorType == (int)OperatorType.LessThanOrEqual && rec.Value > rule.Value))
                    {
                        warningState = (int)WarningState.GasLeakage;
                    }
                }
            }
            if (currentSensor.WarningState == 0 && warningState != 0)
            {
                warningState = (int)WarningState.PreWarning;
            }
            else if (currentSensor.WarningState == (int)WarningState.PreWarning && currentSensor.ActiveTime != null)
            {
                DateTime Now = DateTime.UtcNow.AddHours(7);
                TimeSpan timeSpan = Now.Subtract((DateTime)currentSensor.ActiveTime);
                if (timeSpan.TotalSeconds <= (int)Dictionary.Time.DelayTime)
                {
                    return null;
                }
            }
            return warningState;

        }

        public SensorWebViewModel GetWebViewSensorValue(SensorViewModel model)
        {
            SensorWebViewModel responseModel = new SensorWebViewModel(model);
            Sensor sensor = sensorReposiotory.GetAll().Where(x => x.SensorId.Equals(model.SensorId)).FirstOrDefault();
            int sensorType = sensor.SensorType.SensorType1;
            if (sensorType == (int)Dictionary.SensorType.Fire ||
                sensorType == (int)Dictionary.SensorType.Gas ||
                sensorType == (int)Dictionary.SensorType.Motion ||
                sensorType == (int)Dictionary.SensorType.Smoke ||
                sensorType == (int)Dictionary.SensorType.WaterLeak)
            {
                if (model.Value == 0)
                {
                    responseModel.Value = "Not Detect";
                }
                else
                {
                    responseModel.Value = "Detect";
                }
            }
            else if (sensorType == (int)Dictionary.SensorType.Electric ||
                sensorType == (int)Dictionary.SensorType.Light)
            {
                if (model.Value == 0)
                {
                    responseModel.Value = "No";
                }
                else
                {
                    responseModel.Value = "Yes";
                }
            }
            else
            {
                responseModel.Value = model.Value.ToString();
            }

            return responseModel;
        }

        public SensorsWebInfoViewModel SaveData(SensorsDataViewModel model)
        {
            DateTime Now = DateTime.UtcNow.AddHours(7);
            INotificationBusiness notification = new NotificationBusiness();

            Device currentDevice = deviceReposiotory.GetAll().Where(x => x.DeviceId.Equals(model.DeviceId.Trim())).FirstOrDefault();
            List<Sensor> sensorList = sensorReposiotory.GetAll().ToList();

            SensorsWebInfoViewModel responseModel = new SensorsWebInfoViewModel(model);
            
            if (currentDevice == null)
                return null;
            else
            {
                currentDevice.ActiveTime = Now;
                currentDevice.State = model.State;
                deviceReposiotory.Update(currentDevice);
            }

            for (int i = 0; i < model.SensorList.Count; i++)
            {
                var item = model.SensorList[i];
                var currentSensor = sensorList.Find(x => x.SensorId.Equals(item.SensorId.Trim()));
                if (currentSensor != null)
                {
                    var record = new APMS.DataAccess.Record();
                    record.SensorId = item.SensorId;
                    record.Value = (double)item.Value;
                    record.Time = Now;
                    record.State = model.State;
                    recordReposiotory.Insert(record);
                    int? warningState = GetWarningStateByValue(record, (int)currentDevice.State);
                    model.SensorList[i].WarningState = warningState;

                    if (warningState != null)
                    {
                        if (warningState != currentSensor.WarningState)
                        {
                            currentSensor.WarningState = (int)warningState;
                            if (warningState != (int)WarningState.Normal && warningState != (int)WarningState.PreWarning)
                            {
                                notification.SendWarningSensorStateNotification(currentSensor);
                            }
                        }
                        currentSensor.Value = (double)item.Value;
                        currentSensor.ActiveTime = Now;
                        sensorReposiotory.Update(currentSensor);
                    }
                }
                responseModel.SensorList.Add(GetWebViewSensorValue(item));
            }

            return responseModel;
        }
    }
}
