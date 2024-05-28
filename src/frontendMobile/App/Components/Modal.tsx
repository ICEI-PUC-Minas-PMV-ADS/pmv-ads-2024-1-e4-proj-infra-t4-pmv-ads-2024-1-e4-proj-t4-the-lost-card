import {StyleSheet, View} from 'react-native';
import LostCardIcon from '../Assets/logo.svg';
import React from 'react';
import {ViewProps} from 'react-native-svg/lib/typescript/fabric/utils';

const LostCardModal: React.FC<ViewProps> = ({children, ...props}) => {
  return (
    <View style={styles.container} >
      <LostCardIcon width={'40%'}/>
      <View style={styles.verticleLine} />
      <View {...props}>{children}</View>
    </View>
  );
};

export default LostCardModal;

const styles = StyleSheet.create({
  container: {
    width: '80%',
    height: '75%',
    flexDirection: 'row',
    justifyContent: 'space-around',
    alignItems: 'center',
    backgroundColor: '#5E5C72',
    borderRadius: 10,
    padding: 20
  },
  verticleLine: {
    height: '90%',
    width: 1,
    backgroundColor: '#909090',
  },
});
