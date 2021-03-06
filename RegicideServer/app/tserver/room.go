package tserver

import (
	GameProto "RegicideServer/Gameproto"
	"errors"
	"math/rand"
	"strconv"
	"time"

	"github.com/wonderivan/logger"
)

type Room struct {
	ClientList []*Client
	RoomPack   *GameProto.RoomPack
}

type BossActor struct {
	MaxHp     int32
	Hp        int32
	Atk       int32
	JokerAtk  bool
	CardInt   int32
	CardValue int32
	CardType  int32
}

var roomID int32 = 1000

func InstanceRoom(roomPack *GameProto.RoomPack) Room {
	room := Room{RoomPack: roomPack}
	return room
}

func CreateRoom(roomName string) Room {
	roomID = roomID + 1

	mainPack := &GameProto.MainPack{}
	mainPack.Requestcode = GameProto.RequestCode_Room
	roompack := &GameProto.RoomPack{}
	roompack.Roomname = roomName
	roompack.Maxnum = 4
	roompack.RoomID = roomID
	room := InstanceRoom(roompack)
	RoomList = append(RoomList, &room)
	return room
}

func CreateRooms() {
	for i := 0; i < 1; i++ {
		mainPack := &GameProto.MainPack{}
		mainPack.Requestcode = GameProto.RequestCode_Room
		roompack := &GameProto.RoomPack{}
		roompack.Roomname = "1"
		roompack.Maxnum = 999
		room := InstanceRoom(roompack)
		RoomList = append(RoomList, &room)
	}
}

func (room *Room) RemoveClient(client *Client) error {
	if room == nil {
		return errors.New("room is nil")
	}

	mainPack := &GameProto.MainPack{}
	mainPack.Requestcode = GameProto.RequestCode_Room
	mainPack.Actioncode = GameProto.ActionCode_UpCharacterList

	mainPack.Str = client.Username

	room.Broadcast(mainPack)

	room.ClientList = RemoveC(ClientList, client)
	room.RoomPack.Curnum = room.RoomPack.Curnum - 1
	logger.Debug("Rmv client from Room =>", client.Addr, "Uniid :=>", client.Uniid, "  ClientCount =>", len(room.ClientList))
	return nil
}

func (room *Room) Join(client *Client) {
	client.RoomInfo = room

	for i := 0; i < len(room.ClientList); i++ {
		if room.ClientList[i] == client {
			return
		}
	}

	room.ClientList = append(room.ClientList, client)
	room.RoomPack.Curnum = room.RoomPack.Curnum + 1
	// room.InitCards()
	// room.InitMyCards()
}

//boss
func (room *Room) InitBoss() *GameProto.ActorPack {
	r := rand.New(rand.NewSource(time.Now().UnixNano()))
	count := len(BossList)
	index := r.Intn(count - 1)

	var atk int32
	var hp int32

	cardData := BossList[index]
	if cardData.CardValue == 11 {

	}

	bossActor := &GameProto.ActorPack{ATK: atk, Hp: hp}
	return bossActor
}

func (room *Room) StartGame(client *Client) {
	gameState := &GameProto.GameStatePack{}
	gameState.State = GameProto.GAMESTATE_STATE1
	room.RoomPack.Gamestate = gameState

	mainPack := &GameProto.MainPack{}
	mainPack.Requestcode = GameProto.RequestCode_Room
	mainPack.Actioncode = GameProto.ActionCode_StartGame

	for i := 0; i < len(room.ClientList); i++ {
		_client := room.ClientList[i]
		playerpack := &GameProto.PlayerPack{}
		playerpack.Playername = strconv.Itoa(int(_client.Uniid)) //todo _client.Username
		playerpack.PlayerID = strconv.Itoa(int(_client.Uniid))
		mainPack.Playerpack = append(mainPack.Playerpack, playerpack)
	}
	mainPack.Roompack = append(mainPack.Roompack, room.RoomPack)
	room.Broadcast(mainPack)
}

//??????
func (room *Room) Starting(client *Client) {
	gameState := &GameProto.GameStatePack{}
	gameState.State = GameProto.GAMESTATE_STATE1
	room.RoomPack.Gamestate = gameState

	mainPack := &GameProto.MainPack{}
	mainPack.Requestcode = GameProto.RequestCode_Room
	mainPack.Actioncode = GameProto.ActionCode_Starting

	for i := 0; i < len(room.ClientList); i++ {
		_client := room.ClientList[i]
		playerpack := &GameProto.PlayerPack{}
		playerpack.Playername = _client.Username
		playerpack.PlayerID = strconv.Itoa(int(_client.RoleId))

		mainPack.Playerpack = append(mainPack.Playerpack, playerpack)
	}
	room.Broadcast(mainPack)
}

func (room *Room) Broadcast(mainPack *GameProto.MainPack) {
	for i := 0; i < len(room.ClientList); i++ {
		room.ClientList[i].SendTCP(mainPack)
	}
}

func (room *Room) BroadcastTCP(client *Client, mainPack *GameProto.MainPack) {
	if room == nil {
		logger.Emer("Broadcast failed room is nil!!!")
		return
	}
	for i := 0; i < len(room.ClientList); i++ {
		if room.ClientList[i] == client {
			continue
		}
		if room.ClientList[i] == nil {
			continue
		}
		room.ClientList[i].SendTCP(mainPack)
	}
}

//Cards
func (room *Room) InitCards() {
	for i := 0; i < 54; i++ {
		cardData := InstanceCardData(i)
		ToTalCardList = append(ToTalCardList, &cardData)
	}
}

func (room *Room) InitMyCards() {
	for i := 0; i < len(ToTalCardList); i++ {
		cardData := ToTalCardList[i]
		if cardData.IsBoss {
			BossList = append(BossList, cardData)
		} else {
			MyCardList = append(MyCardList, cardData)
		}
	}
	RandomSort(MyCardList)
	logger.Debug(MyCardList)
}

func (room *Room) TurnCards(client *Client) {

}

func RandomSort(cardDatas []*CardData) {
	r := rand.New(rand.NewSource(time.Now().UnixNano()))
	count := len(cardDatas)
	for i := 0; i < count; i++ {
		index := r.Intn(count - 1)
		temp := cardDatas[i]
		cardDatas[i] = cardDatas[index]
		cardDatas[index] = temp
	}
}

var ToTalCardList []*CardData
var MyCardList []*CardData
var BossList []*CardData

type CardData struct {
	CardInt   int
	CardValue int
	CardType  int
	IsBoss    bool
	IsJoker   bool
	IsPet     bool
}

func InstanceCardData(cardInt int) CardData {

	cardValue := 0
	if cardInt == 52 || cardInt == 53 {
		cardValue = 0
	} else {
		cardValue = (cardInt % 13) + 1
	}
	cardType := 0
	if cardValue == 0 {
		cardType = 5
	} else {
		cardType = ((cardInt) / 13) + 1
	}

	isJoker := cardType == 5
	isBoss := cardValue > 10 && !isJoker
	isPet := cardValue == 1

	cardData := CardData{CardValue: cardValue, CardInt: cardInt, CardType: cardType, IsJoker: isJoker, IsBoss: isBoss, IsPet: isPet}
	return cardData
}
