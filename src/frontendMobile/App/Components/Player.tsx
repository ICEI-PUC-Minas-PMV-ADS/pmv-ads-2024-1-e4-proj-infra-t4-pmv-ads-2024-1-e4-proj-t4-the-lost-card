import { StyleSheet, Text, View } from 'react-native';
import { PropsWithChildren } from 'react';

interface PlayerProps extends PropsWithChildren {
  name: string;
  currentLife: number;
  maxLife: number;
  currentBlock: number;
  energyCurrent?: number;
  maxEnergy?: number;
}

const Player: React.FC<PlayerProps> = ({
  name,
  currentLife,
  maxLife,
  children,
  currentBlock,
  energyCurrent,
  maxEnergy
}) => {
  return (
    <View style={styles.container}>
      <Text>{name}</Text>
      {children}
      <View style={styles.lifeBar}>
        {maxEnergy ? <Text>{energyCurrent}/{maxEnergy}⚡</Text> : null}

        <Text style={styles.lifeText}>HP</Text>
        <View
          style={[
            styles.lifeFulfillment,
            { width: `${(100 * currentLife) / maxLife - 15}%` },
          ]}
        >
          <Text>{`${currentLife}/${maxLife}`}</Text>
        </View>
        {currentBlock > 0 ? <Text>🛡️{currentBlock}</Text> : null}
      </View>
    </View>
  );
};

export default Player;

const styles = StyleSheet.create({
  container: {
    width: 200,
    height: 100,
    justifyContent: 'center',
    alignItems: 'center',
  },
  lifeBar: {
    width: '100%',
    height: 25,
    paddingRight: 5,
    borderRadius: 5,
    flexDirection: 'row',
    backgroundColor: '#4A4A4A',
    alignItems: 'center',
  },
  lifeFulfillment: {
    backgroundColor: '#2FDE52',
    height: '90%',
  },
  lifeText: {
    width: 30,
    textAlign: 'center',
  },
});
