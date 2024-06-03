import {PropsWithChildren} from 'react';
import {StyleSheet, View} from 'react-native';

const CardBase: React.FC<PropsWithChildren> = ({children}) => {
  return <View style={styles.container}>{children}</View>;
};

export default CardBase;

const styles = StyleSheet.create({
  container: {
    height: 140,
    width: 100,
    borderRadius: 5,
    paddingVertical: 20,
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: '#3B4B4A',
  },
});
