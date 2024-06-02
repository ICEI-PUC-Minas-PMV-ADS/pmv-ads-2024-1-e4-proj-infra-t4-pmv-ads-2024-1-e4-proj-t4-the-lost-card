import {StyleSheet, ToastAndroid, TouchableOpacity, View} from 'react-native';
import Background from '../Components/Background';
import LostCardModal from '../Components/Modal';
import LostCardInput from '../Components/Input';
import LostCardButton from '../Components/Button';
import {useContext, useState} from 'react';


const CreateRoom: React.FC = ()=>{
    const [RoomName, setRoomName] = useState('');


    async function createRoomHandler(){

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
