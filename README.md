# Crowd-Control
A Windows service that will automatically logout users that are inactive.

Current status is not production ready. Currently proof of concept/for fun.

Project inspired by: https://www.reddit.com/r/sysadmin/comments/8lbk7o/force_log_off_after_idle_on_shared_computers/

The hope for the service is that you can configure how many inactive users (e.g. disconnected/at lock screen) can exist, and at each new logon, the service will look for inactive users and log them out (log out oldest sessions first).
