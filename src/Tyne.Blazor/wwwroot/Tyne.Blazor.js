function tyneGetTimeZoneName() {
    return Intl.DateTimeFormat().resolvedOptions().timeZone;
}

function tyneGetTimeZoneOffset() {
    return -new Date().getTimezoneOffset();
}
