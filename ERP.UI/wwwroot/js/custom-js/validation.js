

    //fnNumberOnly : To allowdot : 1 if decimal number, allownegative : 1 if allow negative number
    function fnNumberOnly(eventCode, allowdot, allownegative) {
        // numbers only
        if (eventCode >= 48 && eventCode <= 57) {
            return true;
        }
        else if (eventCode == 45 && allownegative == 1) {
            return true;
        }
        else if (allowdot == 1 && eventCode == 46) {
            return true;
        }
        else {
            return false;
        } // else 1
    } // chknum


