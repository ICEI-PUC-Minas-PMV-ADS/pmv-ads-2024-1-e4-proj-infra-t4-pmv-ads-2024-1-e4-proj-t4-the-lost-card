import { StyleSheet, View } from 'react-native';
import Background from '../Components/Background';
import LostCardModal from '../Components/Modal';
import LostCardInput from '../Components/Input';
import LostCardButton from '../Components/Button';
import { useContext, useState } from 'react';
import GameRoomContext from '../Context/gameRoom';
import { JoinGameRoomEventDispatch } from '../Events/Dispatchs/JoinGameRoomEventDispatch';
import { JoinGameRoomEventListener, JoinGameRoomEventListenerContent } from '../Events/Listeners/JoinGameRoomEventListener';
import AuthContext from '../Context/auth';
import { useNavigation } from '@react-navigation/native';
import { StackNavigation } from '../Routes/app.routes';


const CreateRoom: React.FC = () => {
  const navigation = useNavigation<StackNavigation>();
  const [roomName, setRoomName] = useState('');
  const { user } = useContext(AuthContext);
  const { start, setRoom, dispatch, ensureListener } = useContext(GameRoomContext);

  async function createRoomHandler() {
    const connection = await start();
    if (!connection) {
      throw new Error('Connection is not started');
    }

    const joinGameRoomListener = new JoinGameRoomEventListener(setRoom, user!.name);

    ensureListener<JoinGameRoomEventListener, JoinGameRoomEventListenerContent>(joinGameRoomListener)

    const joinGameRoomDispatch = new JoinGameRoomEventDispatch(null, { RoomName: roomName.length > 0 ? roomName : "Public lobby" });

    dispatch(joinGameRoomDispatch, connection)

    navigation.navigate('GameRoomRouter');
  }

  return (
    <Background>
      <LostCardModal>
        <View style={styles.form}>
          <LostCardInput placeholder={'Nome da Sala'} onChangeText={setRoomName}>
          </LostCardInput>
          <View>
            <LostCardButton text={'Criar Sala'} onPress={createRoomHandler} />
          </View>
        </View>
      </LostCardModal>
    </Background>
  );
}

export default CreateRoom;

const styles = StyleSheet.create({
  form: {
    gap: 15,
    justifyContent: 'center',
    alignItems: 'center',
  },
});
