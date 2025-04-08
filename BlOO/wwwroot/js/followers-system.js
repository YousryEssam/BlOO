// buttons
var follow_btn;
var unfollow_btn;
var data;
var profileId
var currentUserId
var FollowersCount;
function IsFollower(profileId, userId)
{
    connection.invoke("IsFollower", profileId, userId).catch(function (err)
    {
        return console.error(err.toString());
    });
}

function UnfollowUser()
{
    connection.invoke("UnfollowUser", profileId, currentUserId).catch(function (err)
    {
        return console.error(err.toString());
    });

    FollowersCount--;
    document.getElementById("FollowersCount").innerHTML = FollowersCount;
}
function FollowUser()
{

    connection.invoke("FollowUser", profileId, currentUserId).catch(function (err)
    {
        return console.error(err.toString());
    });

    FollowersCount++;
    document.getElementById("FollowersCount").innerHTML = FollowersCount;
}


// Create Connection to followers hub
var connection = new signalR.HubConnectionBuilder().withUrl("/FollowersSystem").build();

connection.start().then(function ()
{
    console.log("Connected To Followers System successfully");
    data = document.getElementById("user-data");
    FollowersCount = parseInt(document.getElementById("FollowersCount").innerHTML);
    profileId = parseInt(data.dataset.profileId);
    currentUserId = parseInt(data.dataset.currentUser);
    IsFollower(profileId, currentUserId);

}).catch(function (err)
{
     return console.error(err.toString());
});


connection.on("CheekFollower", function (isFollower)
{
    console.log("User follow this profile : " + isFollower);
    if (isFollower)
    {
        unfollow_btn.classList.toggle("visually-hidden");
    }
    else {
        follow_btn.classList.toggle("visually-hidden");
    }
});

connection.on("ChangeFollowStata", function ()
{

    follow_btn.classList.toggle("visually-hidden");
    unfollow_btn.classList.toggle("visually-hidden");

});



///////////////////////////////////////////////////////////////
document.addEventListener("DOMContentLoaded", function ()
{
    follow_btn = document.getElementById("follow-btn");
    unfollow_btn = document.getElementById("unfollow-btn");
});