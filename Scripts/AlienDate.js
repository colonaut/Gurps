var AlienDate;

(function () {
    var _defaultCalendar = {
        yeardays: 365, //How many days has one year
        leapyeardays: 366, //how many days has one leap year
        leapyear: [4, 100, 400], //when does a leap year occur
        months: [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31], //how many months in a year and how many days in each month
        leapmonths: [31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31], //how many months in a leap year and how many days in each month
        weekdays: 7, //how many days has a week
        hours: 24,
        minutes: 60,
        seconds: 60
    };
    var _defaultCulture = {
        week: ['Sun', 'Mon ', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
        months: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        calendarRule: -1, //default -1, index, (4 = thursday, european, -1 = none, american) //when does the calendar start accounting
        offset: 0 // default 0, (1 = european, 0 = american, -1 = hebrew) //the first day (index) of the week
    };
    var _calendar = _defaultCalendar; //TODO set this better? needed?
    var _culture = _defaultCulture; //TODO set this better? needed?


    var _cultureInfo = {
        
    };

    /* needed for date object (Gregorian- and AlienDate) */

    function _getDateObject(date) { //scope calendar
        var year = date.getUTCFullYear();
        var isleapyear = _isLeapYear.call(this, year);
        var months = isleapyear ? this.leapmonths : this.months;
        var month = date.getUTCMonth();
        var monthDay = date.getUTCDate();
        var yearDay = 0;
        var mix;
        for (mix = 0; mix < month; mix++) {
            yearDay += months[mix];
        }
        yearDay += monthDay;
        //console.log('year:' + year + ' yearDay:' + yearDay + ' monthDay:' + monthDay + ' month:' + month + ' weekDay:' + date.getUTCDay() + ' time:' + date.getUTCHours() + ':' + date.getUTCMinutes() + ':' + date.getUTCSeconds() + ':' + date.getUTCMilliseconds() + ' leapyear:' + isleapyear);
        return {
            year: year,
            yearDay: yearDay,
            month: month,
            monthDay: monthDay,
            monthDays: months[month],
            weekDay: date.getUTCDay(),
            hour: date.getUTCHours(),
            minute: date.getUTCMinutes(),
            second: date.getUTCSeconds(),
            ms: date.getUTCMilliseconds(),
            isLeapYear: isleapyear
        };
    }

    function _isLeapYear(year) {//scope calendar
        return (year % this.leapyear[2] == 0 || (year % this.leapyear[1] != 0 && year % this.leapyear[0] == 0)); //TODO: modolos must be nullable
    }

    function _modifyMonthDays(modifier, dateObject) { //scope calendar
        //console.log('modifier:' + modifier);
        var year = dateObject.year;
        while (modifier < 0) {
            //console.log('< 0 + c.m.l');
            year--;
            modifier += this.months.length;
        }
        modifier += dateObject.month;
        while (modifier >= this.months.length) {
            //console.log('> c.m.l - c.m.l');
            year++;
            modifier -= this.months.length;
        }
        //console.log('modified: ' + year + ' ' + modifier);
        return _isLeapYear.call(this, year) ? this.leapmonths[modifier] : this.months[modifier];
    }

    function _modifyWeekDay(modifier, dateObject) { //scope calendar
        var weekday = dateObject.weekDay;
        var weekdays = this.weekdays;
        return (weekdays + ((weekday + modifier) % weekdays)) % weekdays;
    }
    
    function _modifyYearDays(modifier, dateObject) { //scope calendar
        var year = dateObject.year + modifier;
        return _isLeapYear.call(this, year) ? this.leapyeardays : this.yeardays;
    }


    /*****
        GregorianDate constructor and methods
    *****/
    function GregorianDate(date) {
        var calendar = _calendar;

        var msMinute = 60000; //should go somewhere else (also in alien date)
        var msHour = 60 * msMinute;//should go somewhere else (also in alien date)
        var msDay = 24 * msHour;//should go somewhere else (also in alien date)
        
        var obj = _getDateObject.call(calendar, date);

        this.family = 'GregorianDate';
        this.toString = function () { return '[object ' + this.family + ']'; };

        this.year = function () {//Returns the year (four digits)
            return obj.year;
        };
        this.yearDay = function() {//returns the day of the year
            return obj.yearDay;
        };
        this.yearDays = function (modifier) { //modifier usually is undefined or 0 (same effect). -/+ numbers will get the number of the yearDays of +n/-n years
            if (typeof modifier == 'number') {
                return _modifyYearDays.call(calendar, modifier, obj);
            }
            return calendar.yeardays;
        };
        this.monthDay = function() { //number of the monthday
            return obj.monthDay;
        };
        this.monthDays = function(modifier) { //modifier usually is undefined or 0 (same effect). -/+ numbers will get the number of the monthDays of +n/-n months
            if (typeof modifier == 'number') {
                return _modifyMonthDays.call(calendar, modifier, obj);
            }
            return obj.monthDays;
        };
        this.month = function () { //index of the month
            return obj.month;
        };
        this.months = function() {
            return calendar.months.length;
        };
        this.weekDay = function (modifier) { //index of the weekday, //modifier usually is undefined or 0 (same effect). -/+ numbers will get the week day from current week day +/- modifier in days
            if (typeof modifier == 'number') {
                return _modifyWeekDay.call(calendar, modifier, obj);
            }
            return obj.weekDay;
        };
        this.weekDays = function() { //number of the weekdays
            return calendar.weekdays;
        };
        
        this.between = function () {
            
        };
    }

    GregorianDate.culture = function (culture) {
        if (culture)
            _culture = culture;
        return _culture || _defaultCulture;
    };


    /* needed for date object (AlienDate) */

    function _getDateObjectByTime(msTime) {//scope calendar
        /********
        //0 in ms is set to the 01.01.01 00:00:00:00
        //which is 24 * 60 * 60 * 1000 * 365
        //gregorian timestamp 1.1.1970 1:0:0 719527*24*60*60*1000 + 60 * 60 *1000
        calcTime = 719527 * 24 * 60 * 60 * 1000; //gregorian start of calculation (01.01.1970 00:00:00)
        var realTime = calcTime - (24 * 60 * 60 * 1000 * 365); //gregoriian converted to AlienCalendar Time
        console.log('_getDateObject calcTime:' + calcTime); //62135596800000
        console.log('_getDateObject realTime:' + realTime); //
        ********/
        var seconds = parseInt(msTime / 1000, 10);
        var minutes = parseInt(seconds / 60, 10);
        var hours = parseInt(minutes / 60, 10);
        var passedDays = parseInt(hours / 24, 10);
        var hour = hours - passedDays * 24;
        var minute = minutes - hours * 60;
        var second = seconds - minutes * 60;
        var millisecond = msTime - seconds * 1000;
        //calculate year and day of year
        var year = 0, yearDay = passedDays + 1, isleapyear = false;
        while (yearDay > (isleapyear ? this.leapyeardays : this.yeardays)) {
            year++;
            if (isleapyear) {
                yearDay = yearDay - this.leapyeardays;
            } else {
                yearDay = yearDay - this.yeardays;
            }
            isleapyear = _isLeapYear.call(this, year);
        }
        var weekDay = passedDays % this.weekdays;
        var months = isleapyear ? this.leapmonths : this.months;
        var monthDay = yearDay;
        var month;
        for (month = 0; month < months.length; month++) {
            if (months[month] > monthDay) { break; }
            monthDay = monthDay - months[month];
        }
        //console.log('year:' + year + ' yearDay:' + yearDay + ' monthDay:' + monthDay + ' month:' + month + ' weekDay:' + weekDay + ' time:' + hour + ':' + minute + ':' + second + ':' + millisecond + ' leapyear:' + isleapyear + ' days (passed until):' + passedDays);
        return {
            year: year,
            yearDay: yearDay,
            month: month,
            monthDay: monthDay,
            monthDays: months[month],
            weekDay: weekDay,
            hour: hour,
            minute: minute,
            second: second,
            ms: millisecond,
            isLeapYear: isleapyear,
            passedDays: passedDays
        };
    }

    function _getDateObjectByValues(dateTime) {
        //calc year time
        var year = 0;
        var passedDays = 0;
        var yearDay = 0;
        var monthDay = dateTime[2];
        var isleapyear = false;
        var weekDay, months, month;
        while (year < dateTime[0]) {
            year++;
            passedDays += isleapyear ? this.leapyeardays : this.yeardays;
            isleapyear = _isLeapYear.call(this, year);
        }
        months = isleapyear ? this.leapmonths : this.months;
        for (month = 0; month < dateTime[1]; month++) {//start with one, as this is not index but rno.
            yearDay += months[month];
            passedDays += months[month];
        }
        yearDay += monthDay;
        passedDays += monthDay - 1; //as a day alwas is not yet finished, so only -1 days have completely passed!
        weekDay = passedDays % this.weekdays;
        //console.log('year:' + year + ' yearDay:' + yearDay + ' monthDay:' + monthDay + ' month:' + month + ' weekDay:' + weekDay + ' time:' + dateTime[3] + ':' + dateTime[4] + ':' + dateTime[5] + ':' + dateTime[6] + ' leapyear:' + isleapyear + ' days (passed until):' + passedDays);
        return {
            year: year,
            yearDay: yearDay,
            weekDay: weekDay,
            month: month,
            monthDay: monthDay,
            monthDays: months[month],
            hour: dateTime[3],
            minute: dateTime[4],
            second: dateTime[5],
            ms: dateTime[6],
            isLeapYear: isleapyear,
            pd: passedDays
        };
    }


    /*****
    AlientDate constructor and methods
    *****/
    AlienDate = function (calendar, time) {
        time = (typeof calendar == 'number' || typeof calendar == 'string') ? calendar : (time || 0); //we start at least 0, if nothing is given
        calendar = typeof calendar == 'object' ? calendar : _calendar;
        var msMinute = calendar.seconds * 1000; //move somewhere else
        var msHour = calendar.minutes * calendar.seconds * 1000; //move somewhere else
        var msDay = msHour * calendar.hours; //move somewhere else
        var calcZeroToDayOne = msDay * calendar.yeardays; //this will increase the given time starting at 0 to first day in year one. (leapyear is not relevant as first year cannot be a leapyear (modulo 1 doesn't make sense)
        var calcTime;
        var obj;
        //methods
        this.family = 'AlienDate';
        this.toString = function () {
            return 'object [' + this.family + ']';
        };
        this.time = function () {//Returns the number of milliseconds since 01.01 00:00:00
            //console.log(calcTime + ' - ' + calcZeroToDayOne);

            return calcTime - calcZeroToDayOne;
        };
        this.year = function () {//Returns the year (four digits)
            return obj.year;
        };
        this.yearDay = function () {//returns the day of the year
            return obj.yearDay;
        };
        this.yearDays = function (modifier) { //modifier usually is undefined or 0 (same effect). -/+ numbers will get the number of the yearDays of +n/-n years
            if (typeof modifier == 'number') {
                return _modifyYearDays.call(calendar, modifier, obj);
            }
            return calendar.yeardays;
        };
        this.monthDay = function () { //number of the monthday
            return obj.monthDay;
        };
        this.monthDays = function (modifier) { //modifier usually is undefined or 0 (same effect). -/+ numbers will get the number of the monthDays of +n/-n months
            if (typeof modifier == 'number') {
                return _modifyMonthDays.call(calendar, modifier, obj);
            }
            return obj.monthDays;
        };
        this.month = function () { //index of the month
            return obj.month;
        };
        this.months = function () {
            return calendar.months.length;
        };
        this.weekDay = function (modifier) { //index of the weekday, //modifier usually is undefined or 0 (same effect). -/+ numbers will get the week day from current week day +/- modifier in days
            if (typeof modifier == 'number') {
                return _modifyWeekDay.call(calendar, modifier, obj);
            }
            return obj.weekDay;
        };
        this.weekDays = function () { //number of the weekdays
            return calendar.weekdays;
        };
        this.hour = function (hour) {//Returns the hour (from 0-23) or sets it
            if (!isNaN(hour)) {
                calcTime = calcTime + (hour * msHour - obj.hour * msHour);
                obj.hour = hour;
                return this;
            }
            return obj.hour;
        };
        this.month = function (month) { //Returns the month (from 0-11)
            if (!isNaN(month)) {
                var months = obj.isLeapYear ? calendar.leapmonths : calendar.months;
                var mix;
                if (month < obj.month) {
                    for (mix = month; mix < obj.month; mix++) { calcTime = calcTime - (months[mix] * msDay); }
                } else if (month > obj.month) {
                    for (mix = obj.month; mix < month; mix++) { calcTime = calcTime + (months[mix] * msDay); }
                }
                obj.month = month;
                return this;
            }
            return obj.month;
        };
        this.minute = function (minute) {//Returns the minutes (from 0-59) or sets it
            if (!isNaN(minute)) {
                calcTime = calcTime + (minute * msMinute - obj.minute * msMinute);
                obj.minute = minute;
                return this;
            }
            return obj.minute;
        };
        this.second = function (second) {//Returns the seconds (from 0-59) or sets it
            if (!isNaN(second)) {
                calcTime = calcTime + (second * 1000 - obj.second * 1000);
                obj.second = second;
                return this;
            }
            return obj.second;
        };
        this.ms = function (ms) {
            if (!isNaN(ms)) {
                calcTime = calcTime + (ms - obj.ms);
                obj.ms = ms;
                return this;
            }
            return obj.ms;
        };

        this.between = function (alienDate) {
            //set the alienDate to the current AlienDate hrs mins and secs.
            alienDate.hour(this.hour()).minute(this.minute()).second(this.second()).ms(this.ms());
            var msBetween = this.time() - alienDate.time();
            msBetween = msBetween < 0 ? msBetween * -1 : msBetween;
            return msBetween / msDay; //days
            return msBetween / msHour //hours
            return msBetween / msMinute //minutes
            return msBetween / 1000 //seconds
        };

        switch (typeof time) {
            case 'number': //ms
                calcTime = time + calcZeroToDayOne;
                //*******
                //convert input ms to gregorian UTC:
                calcTime = calcTime - (24 * 60 * 60 * 1000 * 365) + 719527 * 24 * 60 * 60 * 1000;
                //********/
                obj = _getDateObjectByTime.call(calendar, calcTime);
                return this;
            case 'string': //a dateString (iso)
                var dateTime = [1, 1, 1, 0, 0, 0, 0, 'Z'];
                var whatTime = time.match(/[T| ](\d{2}):(\d{2}):(\d{2}).(\d{3})([A-z])/) || [0, 0, 0, 0, 'Z']; //iso time
                //console.log(whatTime);
                var whichDate = time.match(/(\d{4})-(\d{2}|\d{1})-(\d{2}|\d{1})/) || [1, 1, 1]; //iso date
                //console.log(whichDate);
                if (whatTime) {
                    for (var i = 1; i < whatTime.length - 1; i++) {
                        dateTime[i + 2] = parseInt(whatTime[i], 10);
                    }
                    dateTime[7] = whatTime[5] || dateTime[7];
                }
                if (whichDate) {
                    for (var i = 1; i < whichDate.length; i++) {
                        dateTime[i - 1] = parseInt(whichDate[i], 10);
                    }
                }
                dateTime[1]--;
                //console.log(dateTime);
                obj = _getDateObjectByValues.call(calendar, dateTime); //TODO: time, how best bring calculated time to object... put time into object?
                calcTime = obj.pd * msDay + obj.hour * msHour + obj.minute * msMinute + obj.second * 1000;
                return this;
            default:
                throw new Error("unsupported timestamp format");
                return this;
        }

    };

    AlienDate.calendar = function (calendar) {
        if (calendar)
            _calendar = calendar;
        return _calendar;
    };

    AlienDate.culture = function (culture) {
        if (culture)
            _culture = culture;
        return _culture || _defaultCulture;
    };


    /* needed for calendar creation (plugin functions) */

    function _mapWeekOffset() {//scope culture
        var offset = this.offset;
        var weekLength = this.week.length;
        return this.week.map(function (value, index) {
            return (index + weekLength + offset) % weekLength;
        });
    }

    function _getMonthMatrix(date) { //scope culture
        //console.log('_getMonthMatrix (based on culture)');
        var offset = this.offset;
        var week = this.week;
        var monthBeforeDays, monthAfterDays, yearFirstWeekDay, yearDaysToCalendarRuleDay, yearLastWeekDay, monthFirstWeekNo, monthLastWeekNo, weekNo;
        var weekOffset = _mapWeekOffset.call(this); //the offset mapped weekdays array
        var weekDaysToMonthDay, weekDaysBeforeMonth, weekDaysAfterMonth; //these need offset calculation for correct weekday display
        var pDays, pWeekDays, pMonthDays, pMonthDaysSwitchAt;
        var pIsActive = false;
        var matrix = {
            year: date.year(),
            monthName: this.months[date.month()],
            weekDayNames: [],
            weeks: []
        }, matrixWeek;
        
        monthBeforeDays = date.monthDays(-1);
        //console.log('monthBeforeDays: ' + monthBeforeDays);
        monthAfterDays = date.monthDays(1);
        //console.log('monthAfterDays: ' + monthAfterDays);
        weekNo = Math.ceil(date.yearDay() / date.weekDays());
        //apply calendar rule (offset, calendarRule day index)
        if (this.calendarRule > -1) { //only if a calendarRule applies, we use it for weekNo
            yearFirstWeekDay = date.weekDay((date.yearDay() - 1) * -1); //we decrease yearday -1 because current day is not yet over!
            //console.log('first week day of year: ' + week[yearFirstWeekDay]);
            yearDaysToCalendarRuleDay = (date.weekDays() - (yearFirstWeekDay - this.calendarRule)) % date.weekDays();
            //console.log('days to first calenderruleday: ' + yearDaysToCalendarRuleDay);
            //console.log('1.4 + ' + (date.yearDay() - yearDaysToCalendarRuleDay) / date.weekDays()); //round correct! (usual +1.5, but we use formula on standard date, so... we might use the offset: // 1.5 + (offset / 10) -> 1.4 || 1.6, etc.
            //console.log(1.4 + (date.yearDay() - yearDaysToCalendarRuleDay) / date.weekDays());
            weekNo = Math.floor(1.4 + ((date.yearDay() - yearDaysToCalendarRuleDay) / date.weekDays()));
            //console.log(weekNo);
            if (weekNo == 0) { //this is the first week of the _year_ and as it's not  it must get the value of lat year's last week;
                monthFirstWeekNo = date.yearDays(-1) + yearDaysToCalendarRuleDay;
                monthFirstWeekNo = (monthFirstWeekNo - (monthFirstWeekNo % 7)) / 7;
                console.log(monthFirstWeekNo);
            } else if (date.month() == date.months() - 1) {
                yearLastWeekDay = date.weekDay(date.yearDays() - date.yearDay());
                //console.log('yearLastWeekDay: ' + yearLastWeekDay + ' ' + this.week[yearLastWeekDay]);
                //console.log('formula: 7+' + yearLastWeekDay + ' - ' + offset + '%7 < ' + '7+' + this.calendarRule + ' - ' + offset + '%7');
                //console.log('clculated formula: ' + ((date.weekDays() + yearLastWeekDay - offset) % date.weekDays()) + ' < ' + ((date.weekDays() + this.calendarRule - offset) % date.weekDays()));
                if (((date.weekDays() + yearLastWeekDay - offset) % date.weekDays()) < ((date.weekDays() + this.calendarRule - offset) % date.weekDays())) {
                    monthLastWeekNo = 1;
                }
            }
        }
        
        weekDaysToMonthDay = (date.monthDay() - date.weekDay() + offset) % date.weekDays();
        //console.log('weekDaysToMonthDay: ' + weekDaysToMonthDay);
        weekDaysBeforeMonth = weekDaysToMonthDay;
        while (weekDaysBeforeMonth > 1) { //important: use offset instead of 0 here! //NO! USE 1!
            weekDaysBeforeMonth -= date.weekDays();
        }
        //console.log('weekDaysBeforeMonth:' + weekDaysBeforeMonth);
        weekDaysAfterMonth = (date.weekDays() - ((date.weekDay() + date.monthDays() - date.monthDay() + 1) % date.weekDays()) + offset) % date.weekDays();
        weekDaysAfterMonth++;
        //console.log('weekDaysAfterMonth:' + weekDaysAfterMonth);
        weekOffset.forEach(function (weekDayIndex) {
            matrix.weekDayNames.push(week[weekDayIndex]);
        });  
        //matrixRow = { week: monthFirstWeekNo || weekNo, days: [] };
        matrixWeek = { number: monthFirstWeekNo || weekNo, days: [] };
        pDays = weekDaysBeforeMonth;
        pWeekDays = 0;
        pMonthDays = monthBeforeDays + weekDaysBeforeMonth;
        pMonthDaysSwitchAt = monthBeforeDays;
        while (pDays < (date.monthDays() + weekDaysAfterMonth)) {
            //console.log(pWeekDays % date.weekDays());
            if (pMonthDays > pMonthDaysSwitchAt) {
                pMonthDaysSwitchAt = date.monthDays();
                pMonthDays = 1;
            }
            pIsActive = (pMonthDays == date.monthDay() && pDays > 0 && pDays < pMonthDaysSwitchAt);    //TODO! (checke mit dec 1999, 1 wird hier natürlich zweimal selected)        
            matrixWeek.days.push({ monthDay: pMonthDays, isActive: pIsActive, isRange: true });
            pDays++; pMonthDays++; pWeekDays++;
            if (pWeekDays % date.weekDays() == 0) {
                weekNo++;
                matrix.weeks.push(matrixWeek);
                matrixWeek = { number: weekNo, days: [] };
            }
        }
        if (monthLastWeekNo) {
            matrix.week[matrix.body.length - 1].number = monthLastWeekNo;
        }
        //console.log(matrix);
        return matrix;

    }

    AlienDate.month = function(date) {
        return _getMonthMatrix.call(AlienDate.culture(), date);
    };

})();
