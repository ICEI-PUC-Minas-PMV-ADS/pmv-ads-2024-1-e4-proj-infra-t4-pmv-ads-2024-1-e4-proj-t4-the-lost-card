import React, {PropsWithChildren} from 'react';
import {StyleSheet, Text, Touchable, TouchableOpacity, View} from 'react-native';

interface KawasakiProps extends PropsWithChildren {
    text : string,
    onPress?: () => void;
}

const Kawasaki: React.FC<KawasakiProps> = ({children, text, onPress}) => {
  return (
    <TouchableOpacity style={styles.container} onPress={onPress}>
      <Text>{text}</Text>
      {children}
    </TouchableOpacity>
  );
};

export default Kawasaki;

const styles = StyleSheet.create({
  container: {
    width: 300,
    height: 40,
    justifyContent: 'space-between',
    alignContent: 'center',
    flexDirection: 'row',
    backgroundColor: '#4C4A62',
    borderRadius: 10,
    padding: 10
  },
}); 
