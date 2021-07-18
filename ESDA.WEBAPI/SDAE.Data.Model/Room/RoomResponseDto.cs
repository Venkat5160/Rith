namespace SDAE.Data.Model
{
    public class RoomResponseDto
    {
        public string Token { get; set; }
        public string RoomName { get; set; }
        public string RoomSid { get; set; }
        public string UserId { get; set; }

        public bool IsRoomCreated { get; set; }
        public string Message { get; set; }
    }
}
