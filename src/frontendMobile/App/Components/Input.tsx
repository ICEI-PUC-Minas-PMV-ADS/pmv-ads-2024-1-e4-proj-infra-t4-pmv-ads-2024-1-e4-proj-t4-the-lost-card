import React from 'react';
import {StyleSheet, TextInput, View} from 'react-native';

interface LostCardInputProp extends React.PropsWithChildren {
  placeholder: string;
}

const LostCardInput: React.FC<LostCardInputProp> = ({
  children,
  placeholder,
}) => {
  return (
    <View style={styles.container}>
      <TextInput
        style={styles.input}
        placeholder={placeholder}
        placeholderTextColor="#777777"
      />
      {children}
    </View>
  );
};

const styles = StyleSheet.create({
  input: {
    backgroundColor: 'rgba(0, 0, 0, 0)',
    color: 'white',
    padding: 10,
    fontSize: 16,
    width: 270,
    borderWidth: 0,
  },
  container: {
    display: 'flex',
    flexDirection: 'row',
    justifyContent: 'center',
    alignItems: 'center',
    borderRadius: 1,
    borderBottomWidth: 1,
    borderBottomColor: '#777777',
    width: 300,
  }
});

export default LostCardInput;
