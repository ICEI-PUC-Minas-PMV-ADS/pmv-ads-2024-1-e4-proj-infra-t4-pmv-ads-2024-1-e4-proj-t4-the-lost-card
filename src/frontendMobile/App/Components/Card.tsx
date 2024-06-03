import {StyleSheet, Text, View} from 'react-native';

interface CardProps {
  title: string;
  description: string;
  energy: number;
}

const Card: React.FC<CardProps> = ({description, energy, title}) => {
  return (
    <View style={styles.container}>
      <Text style={styles.energy}>{energy}</Text>
      <Text style={styles.image}>{title}</Text>
      <Text>{description}</Text>
    </View>
  );
};

export default Card;

const styles = StyleSheet.create({
  container: {
    height: 160,
    width: 120,
    borderRadius: 5,
    paddingVertical: 20,
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: '#3B4B4A',
  },
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
