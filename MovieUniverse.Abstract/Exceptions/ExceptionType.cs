namespace MovieUniverse.Abstract.Exceptions
{
    public enum ExceptionType
    {
        UserByIdNotExist,
        UserByThisEmailExist,
        RegistrationError,

        NotFollowForDelete,
        NotFollowable,
        AlreadyFollowed,
        YouCannotFollowYourSelf,

        AlreadyFriends,
        FriendRequestNotExist,
        FriendRequestAlreadySent,
        FriendRequestAlreadyRecived,
        FriendRequestAlreadyRejected,
        YouAreNotFriends,
        YouCannotFriendYourself,

        AlreadyAddedToYourWatchedList,
        AlreadyAddedToYoutWantToWatchList,
        MovieNotExistInYourList,

        UserIsNotPublic,
        InvalidEmailorPassword,
        InvalidPassword,
        InvalidNewPassword,
        EmailisNotConfirmed,
        InvalidKey,
        InvalidRequestAction,
        InvalidActivationKeyOrEmail,
        

        NoSuitableRequestExsit,

        YouHaveRatedThisMovie,
        SameRateStar,


        InvalidProvider,
        YouHaveNoPermission,

        YouAreInAnoterRoom,
        ThisRoomIsInAccessableForYou,
        YouCanInviteOnlyYourFriends,
        OnlyOwnerCanInvitePeople,
        YouHaveOlreadyInvited,
        InvalidInvitationId,
        RoomDoesNotExist,
    }
}