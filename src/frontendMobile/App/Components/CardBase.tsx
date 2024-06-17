import {PropsWithChildren} from 'react';
import {StyleSheet, View} from 'react-native';

interface CardBaseProps extends PropsWithChildren
{
  scaleFactor: number;
}

const CardBase: React.FC<CardBaseProps> = ({scaleFactor,children}) => {
  return <View style={{...styles.container,
    height: styles.container.height * scaleFactor,
    width: styles.container.width * scaleFactor
  }}>{children}</View>;
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
