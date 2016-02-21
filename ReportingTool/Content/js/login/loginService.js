'use strict';

loginModule.service('LoginService', function () {
    alert("service created");
    this.showLogout = false;

    this.SetShowLogoutStatus =  function (showLogout) {
        this.showLogout = showLogout;
    };

    this.GetShowLogoutStatus = function() {
        return this.showLogout;
    };

});


