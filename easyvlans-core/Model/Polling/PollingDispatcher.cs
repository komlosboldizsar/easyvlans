using easyvlans.Model.SwitchOperationMethods;

namespace easyvlans.Model.Polling
{
    public class PollingDispatcher
    {

        private static readonly List<Registration> _registrations = new();

        private class Registration
        {

            public Registration(Switch @switch, string methodCode, PollingSchedule schedule)
            {
                Request = new(@switch, methodCode);
                Schedule = schedule;
                _interval = new TimeSpan(0, 0, Schedule.Interval);
            }

            public readonly PollingRequest Request;
            public readonly PollingSchedule Schedule;
            public DateTime Next { get; set; }
            private readonly TimeSpan _interval;

            public async Task Poll()
            {
                DateTime nextOriginal = Next;
                Next += _interval;
                await PollableMethods.DoRequest(Request);
                Next = nextOriginal + _interval;
            }

        }

        public static void Register(Switch @switch, string methodCode, PollingSchedule schedule)
        {
            if (schedule == null)
                return;
            _registrations.Add(new(@switch, methodCode, schedule));
        }

        public static void RegisterForAll(Switch @switch, PollingScheduleCollection scheduleCollection)
        {
            foreach (string methodCode in PollableMethods.POLLABLE_METHOD_CODES)
                Register(@switch, methodCode, scheduleCollection.Get(methodCode));
        }

        public static void Start()
        {
            initNextTimes();
            Task.Run(tickTask);
        }

        private static void initNextTimes()
        {
            DateTime now = DateTime.Now;
            foreach (Registration registration in _registrations)
                registration.Next = now + new TimeSpan(0, 0, registration.Schedule.Interval + registration.Schedule.Offset);
        }

        private static async Task tickTask()
        {
            while (true)
            {
                tick();
                await Task.Delay(1000);
            }
        }

        private static void tick()
        {
            foreach (Registration registration in _registrations)
                if (registration.Next <= DateTime.Now)
                    Task.Run(registration.Poll);
        }

    }
}
