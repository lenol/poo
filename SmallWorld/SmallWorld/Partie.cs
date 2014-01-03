﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wrapper;

namespace SmallWorld
{
    public class Partie
    {

        public StrategieCarte strat;
        private Carte LaCarte;
        public Joueur joueur1;
        public Joueur joueur2;
        private SelectionOperateur selectOp;
        private Boolean Joueur1ALaMain;


        unsafe public Partie(Joueur j1, Joueur j2, StrategieCarte st)
        {
            joueur1 = j1;
            joueur2 = j2;
            LaCarte = new Carte(st.tailleCarte());
            strat = st;
            int j1x, j1y, j2x, j2y;
            WrapperAlgo.positionJoueurParTaille(getCarte().getLongueurCote(), &j1x, &j1y, &j2x, &j2y);
            joueur1.setx0(j1x);
            joueur1.sety0(j1y);
            joueur2.setx0(j2x);
            joueur2.sety0(j2y);
            LaCarte.InitialisationDeLaCarte(joueur1, joueur2, strat.nombreUniteParPeuple());
            Joueur1ALaMain = true;
            selectOp = new SelectionOperateur();
        }

        public Carte getCarte() {
            return LaCarte;
        }

        public SelectionOperateur getSelectionOperateur() {
            return selectOp;
        }

        public Boolean getJoueur1ALaMain() {
            return Joueur1ALaMain;
        }

        public void changementDeMain()
        {
            Joueur1ALaMain = !Joueur1ALaMain;
        }

        void gestionUnites(List<Unite> unite) { ;}

        void executionTour() { ;}

        void deroulementDeLaPartie() { ;}

        void miseAJourDesPoints() { ;}

        public int combat(Unite unitAtt, Unite target)
        {
            int pvUatt = unitAtt.getPV();
            int pvTarget = target.getPV();
            int att = unitAtt.getAtt()*(pvUatt)/2;
            int def = target.getDef()*(pvTarget)/2;
            int maximum = Math.Max(pvTarget, pvUatt);
            Random r = new Random();
            int nBCombat = r.Next(3,maximum+2);
            Boolean finCombat = false;
            int arret =0;

            for (int i=0;i<nBCombat || !finCombat; i++)
            {
                if ((pvTarget == 0) || (pvUatt == 0))
                    finCombat = true;

                if (haveAttaquePerduUneVie(att, def))
                {
                    pvUatt--;
                    if (0 == pvUatt)
                    {
                        unitAtt.meurt();
                        finCombat = true;
                        arret++;
                    }
                    unitAtt.setPV(pvUatt);
                }
                else
                {

                    pvTarget--;

                    if(0==pvTarget)
                    {
                        target.meurt();
                        unitAtt.majPosition(target.getRow(), target.getColumn());
                        finCombat = true;
                        arret++;
                    }
                    target.setPV(pvTarget);
                }
            }
            return arret;
        }

        public Boolean haveAttaquePerduUneVie(int att, int def)
        {
            Boolean resultat = false;
            double i;
            Boolean attPlusFort = false;
            if (att > def)
                attPlusFort = true;
            i = (1 - att / def) * 100;
            double chance = (i * 25) * 0.5 + 50;
            Random rand = new Random();
            int res = rand.Next(1, 100);
            if (attPlusFort)
            {
                if (res < chance)
                    resultat = true;
            }
            else
            {
                if (res > chance)
                    resultat = true;
            }
            return resultat;
        }
    }
}
