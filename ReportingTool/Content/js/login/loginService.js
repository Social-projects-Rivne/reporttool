'use strict';

loginModule.service('LoginService', function () {
   // alert("service created");
    this.showLogout = {show: true};

    this.SetShowLogoutStatus =  function (showLogout) {
        this.showLogout.show = showLogout;
    };

    this.GetShowLogoutStatus = function() {
        return this.showLogout;
    };

});


