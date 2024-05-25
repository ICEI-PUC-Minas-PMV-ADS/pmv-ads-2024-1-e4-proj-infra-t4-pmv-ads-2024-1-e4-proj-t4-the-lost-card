import {StyleSheet, ToastAndroid, TouchableOpacity, View} from 'react-native';
import Background from '../Components/Background';
import LostCardModal from '../Components/Modal';
import LostCardInput from '../Components/Input';
import EmailIcon from '../Assets/Email.svg';
import PasswordInput from '../Components/PasswordInput';
import LostCardButton from '../Components/Button';
import {useContext, useState} from 'react';
import AuthContext from '../Context/auth';

const SignIn = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const context = useContext(AuthContext);

  async function signIn() {
    console.log(context.user);
    const response = await context.signIn({email, plainTextPassword: password});
    
    if (response === undefined) return;
    
    ToastAndroid.show(response.detail, 2000);
  }
  console.log(context.user);

  return (
    <Background>
      <LostCardModal>
        <View style={styles.form}>
          <LostCardInput placeholder={'Email'} onChangeText={setEmail}>
            <EmailIcon />
          </LostCardInput>
          <PasswordInput placeholder={'Senha'} onChangeText={setPassword} />
          <View>
            <LostCardButton text={'Entrar'} onPress={signIn} />
          </View>
        </View>
      </LostCardModal>
    </Background>
  );
};

export default SignIn;

const styles = StyleSheet.create({
  form: {
    gap: 15,
    justifyContent: 'center',
    alignItems: 'center',
  },
});
