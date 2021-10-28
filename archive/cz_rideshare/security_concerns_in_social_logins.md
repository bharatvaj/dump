Security Concerns in Social Logins
==================================
October 17, 2017
Bharatvaj P H

Overview
--------
Over the past years, the number of apps which is dependent on user data is huge. This led  to the creation of “Social Logins” which helped user to sign in without re-entering his data again and again. Even though it is a real time saver, it must be restricted to some domain not because of the protocol(OAuth2) itself but because of the non verifiability of these Social  Logins.
It is highly not recommended to use Social Logins in Ride Sharing apps.
Reasons
-------
### Non Verifiability
Even though Social Logins are not vulnerable by itself. The credentials of the user is not always authentic in Social Logins. Anyone can create a fake Social profile by using services such as Temp Mail, because Social Sites(Facebook, Twitter, e.t.c.) does not require a mobile number mandatorily for signing up. 

### Loss of Control
As the login is made through a provider, the credentials are valid only as long as the the provider holds the user. In scenarios where the user deletes his Social Profile, he will be automatically logged out of his RideShare app forever. This results in loss of user’s green points and ride history. While this is technically the user’s choice to delete the social profile, it is RideShare to be blamed when he has been been logged out of RideShare too.

> An application using OAuth loses control of their userbase. The true identity of users, and their ability to login, is controlled by the OAuth provider. Your application can be effectively turned off at any time, for any reason. It may not even be a malicious behaviour; I’m sure everybody has had occasional login troubles at Twitter and Facebook. At those times the user is locked out of all their other applications.


Alternative
-----------
While it is impossible to do a background check of every client, the most heuristic solution is by authenticating the user via a mobile number. Since every mobile number is obtained after thorough inspection of the user, it is highly unlikely a mobile number is fake. Even though malicious hackers can get their hands on fake sim cards, the attack vector through sim cards is many times smaller than that of e-mails and Social Logins.
PS: OTP is available via firebase

No Proof But Concept
--------------------
Pathway of a malicious user creating a fake identity in our app.
Recently temp-mail.org has released it’s own API, combine that with python and Facebook Authentication API and we have a Recipe for disaster. Even though Firebase can handle this kind of heat, fake user profiles messes our analytics and most importantly ML which we will be implementing in the next version.

Conclusion
----------
The fake identity of users can make our app susceptible to be used by criminals. To ensure our user community is safe, it is highly recommended not to use Social Login but the good old OTP mobile authentication.

References
----------
https://mortoray.com/2014/02/21/the-dangers-of-oauthsocial-login/
