import React, { useState} from 'react';
import {TextInput, View, TouchableOpacity, StyleSheet} from 'react-native';
import EyeIcon from '../Assets/Eye.svg';
import ClosedEyeIcon from '../Assets/ClosedEye.svg';

interface PasswordInputProps {
  placeholder: string;
}

const PasswordInput: React.FC<PasswordInputProps> = ({placeholder}) => {
  const [show, setShow] = useState(false);

  return (
    <View style={styles.container}>
      <TextInput
        style={styles.field}
        secureTextEntry={!show}
        placeholder={placeholder}
        placeholderTextColor="#777777"
      />
      <TouchableOpacity onPress={() => setShow(!show)} style={styles.eyeIcon}>
        {show ? <EyeIcon /> : <ClosedEyeIcon />}
      </TouchableOpacity>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flexDirection: 'row',
    alignItems: 'center',
    height: 40,
    borderRadius: 1,
    borderBottomWidth: 1,
    borderBottomColor: '#777777',
    width: 300,
  },
  field: {
    backgroundColor: 'rgba(0, 0, 0, 0)',
    color: 'white',
    padding: 10,
    fontSize: 16,
    flex: 1,
  },
  eyeIcon: {
    padding: 10,
  },
});

export default PasswordInput;
