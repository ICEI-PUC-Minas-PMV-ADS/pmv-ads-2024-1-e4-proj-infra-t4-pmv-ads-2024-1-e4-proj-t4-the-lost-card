import {StyleSheet, ToastAndroid, TouchableOpacity, View} from 'react-native';
import Background from '../Components/Background';
import LostCardModal from '../Components/Modal';
import LostCardInput from '../Components/Input';
import EmailIcon from '../Assets/Email.svg';
import AccountIcon from '../Assets/Account.svg';
import PasswordInput from '../Components/PasswordInput';
import LostCardButton from '../Components/Button';
import {useContext, useState} from 'react';
import {signUp} from '../Repositories/SignUpRepository';

const SignUp = ()=>{
    const [username, setUsername] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState(''); 
    const [rePassword, setRePassword] = useState('');
    async function HandlerSignUp() {

        if(password != rePassword){
            ToastAndroid.show("Senhas não condizentes",2000);
            return;
        }
        const response = await signUp({username ,email, plainTextPassword: password});

        if ("id" in response) return console.log(response);

        ToastAndroid.show(response.title, 2000);
    }
    return (
        <Background>
          <LostCardModal>
            <View style={styles.form}>

              <LostCardInput placeholder={'Usuario'} onChangeText={setUsername}>
                <AccountIcon />
              </LostCardInput>
              <LostCardInput placeholder={'Email'} onChangeText= {setEmail}>
                <EmailIcon />
              </LostCardInput>
              <PasswordInput placeholder={'Senha'} onChangeText={setPassword} />
              <PasswordInput placeholder={'Repetir Senha'} onChangeText = {setRePassword}/>
              <View>
                <LostCardButton text={'Cadastrar'} onPress={HandlerSignUp} />
              </View>
            </View>
          </LostCardModal>
        </Background>
      );
    
 
}
export default SignUp;

const styles = StyleSheet.create({
    form: {
      gap: 15,
      justifyContent: 'center',
      alignItems: 'center',
    },
  });

