using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Humica.Logic
{
    public class ZkfpManager
    {
        private zkemkeeper.CZKEM _axCZKEM1;
        private int _iMachineNumber = 1; //the serial number of the device.After connecting the device ,this value will be changed.
        public string IP_Address { get; set; }
        public int Port { get; set; }
        public int CommPort { get; set; }
        public bool ConnectionState { get; set; }
        public string ErrorMessage { get; set; }
        public ZkfpManager()
        {
            _axCZKEM1 = new zkemkeeper.CZKEM();
        }
        public void Connect()
        {
            try
            {
                Task task = Task.Factory.StartNew(() =>
                {
                    int idwErrorCode = 0;
                    if (CommPort != 0)
                    {
                        _axCZKEM1.SetCommPassword(CommPort);
                    }
                    ConnectionState = _axCZKEM1.Connect_Net(IP_Address, Port);
                    _axCZKEM1.GetLastError(ref idwErrorCode);
                    _iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
                    _axCZKEM1.RegEvent(_iMachineNumber, 65535);
                });
                task.Wait();
            }
            catch (Exception ex)
            {
            }
        }
        public IEnumerable<AttendanceRaw> GetRawData(DateTime start, DateTime end)
        {
            if (ConnectionState)
            {
                string sdwEnrollNumber = "";
                //int idwTMachineNumber = 0;
                //int idwEMachineNumber = 0;
                int idwVerifyMode = 0;
                int idwInOutMode = 0;
                int idwYear = 0;
                int idwMonth = 0;
                int idwDay = 0;
                int idwHour = 0;
                int idwMinute = 0;
                int idwSecond = 0;
                int idwWorkcode = 0;
                int idwErrorCode = 0;
                int iGLCount = 0;

                AttendanceRaw attnrw;
                DateTime dt;
                _axCZKEM1.EnableDevice(_iMachineNumber, true); // Enable the device

                if (_axCZKEM1.ReadGeneralLogData(_iMachineNumber)) // Read attendance records
                {
                    while (_axCZKEM1.SSR_GetGeneralLogData(1, out sdwEnrollNumber, out idwVerifyMode,
                           out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))
                    {
                        dt = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond);
                        if (dt.Date >= start.Date && dt.Date <= end.Date)
                        {
                            string enrollNumber;
                            int parsedId;

                            // Safely parse the enrollment number
                            if (int.TryParse(sdwEnrollNumber, out parsedId))
                            {
                                // Format as 4-digit string if ≤4 digits
                                enrollNumber = parsedId.ToString().Length <= 4
                                    ? parsedId.ToString("0000")
                                    : parsedId.ToString();
                            }
                            else
                            {
                                // Use original string if parsing fails
                                enrollNumber = sdwEnrollNumber;
                            }

                            attnrw = new AttendanceRaw { EnrollNumber = enrollNumber, ScanDate = dt };
                            iGLCount++;
                            yield return attnrw;
                        }
                    }
                }
                else
                {
                    _axCZKEM1.GetLastError(ref idwErrorCode);

                    if (idwErrorCode != 0)
                        ErrorMessage = "Reading data from terminal failed, ErrorCode: " + idwErrorCode.ToString();
                    else
                        ErrorMessage = "No data from terminal returns!";
                }
                _axCZKEM1.EnableDevice(_iMachineNumber, true);//enable the device
            }
            else
            {
                ErrorMessage = "Device is not connected!";
            }
        }
        public IEnumerable<AttendanceRaw> GetRawData()
        {
            if (ConnectionState)
            {
                string sdwEnrollNumber = "";
                //int idwTMachineNumber = 0;
                //int idwEMachineNumber = 0;
                int idwVerifyMode = 0;
                int idwInOutMode = 0;
                int idwYear = 0;
                int idwMonth = 0;
                int idwDay = 0;
                int idwHour = 0;
                int idwMinute = 0;
                int idwSecond = 0;
                int idwWorkcode = 0;

                int idwErrorCode = 0;
                int iGLCount = 0;
                //int iIndex = 0;

                AttendanceRaw attnrw;
                DateTime dt;
                _axCZKEM1.EnableDevice(_iMachineNumber, true);//disable the device
                if (_axCZKEM1.ReadGeneralLogData(_iMachineNumber))//read all the attendance records to the memory
                {
                    while (_axCZKEM1.SSR_GetGeneralLogData(1, out sdwEnrollNumber, out idwVerifyMode,
                               out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))//get records from the memory
                    {
                        int ID = Convert.ToInt32(sdwEnrollNumber);
                        dt = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond);
                        attnrw = new AttendanceRaw { EnrollNumber = ID.ToString("0000"), ScanDate = dt };
                        iGLCount++;
                        yield return attnrw;
                    }
                }
                else
                {
                    _axCZKEM1.GetLastError(ref idwErrorCode);

                    if (idwErrorCode != 0)
                        ErrorMessage = "Reading data from terminal failed, ErrorCode: " + idwErrorCode.ToString();
                    else
                        ErrorMessage = "No data from terminal returns!";
                }
                _axCZKEM1.EnableDevice(_iMachineNumber, true);//enable the device
            }
            else
            {
                ErrorMessage = "Device is not connected!";
            }
        }

        public void Disconnect()
        {
            if (ConnectionState)
            {
                _axCZKEM1.Disconnect();
                ConnectionState = false;
            }
        }
    }
}
