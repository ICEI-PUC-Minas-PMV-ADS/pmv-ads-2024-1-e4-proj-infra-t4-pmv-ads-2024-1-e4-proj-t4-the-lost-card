import {StyleSheet, Text, View} from 'react-native';
import CardBase from './CardBase';

interface CardProps {
  title: string;
  description: string;
  energy: number;
}

const Card: React.FC<CardProps> = ({description, energy, title}) => {
  return (
    <CardBase scaleFactor={1}>
      <Text style={styles.energy}>{energy}</Text>
      <Text style={styles.image}>{title}</Text>
      <Text>{description}</Text>
    </CardBase>
  );
};

export default Card;

const styles = StyleSheet.create({
  image: {
    width: '100%',
    height: '40%',
    justifyContent: 'center',
    alignItems: 'center',
  },
  energy: {
    position: 'absolute',
    top: 3,
    left: 3,
    backgroundColor: '#8EEEED',
    width: 20,
    height: 20,
    textAlign: 'center',
    borderRadius: 5,
  },
});
